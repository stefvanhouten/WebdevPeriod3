$(() => {
    const subSystemStore = createSubSystemStore();

    let subSystemIds = new Set();

    subSystemStore.subSystems$.subscribe(subSystems =>
        subSystemIds = new Set(subSystems.map(subSystem => subSystem.id)));

    observeInput("#search-input")
        .pipe(
            rxjs.operators.debounceTime(500),
            rxjs.operators.flatMap(term =>
                !!term ? search(term).then(resultsToHTML) : Promise.resolve(""))
        )
        .subscribe(html => $("#search-output").html(html));

    observeDescendantClick("#search-output", "li[data-id][data-name]")
        .pipe(
            rxjs.operators.map(([element]) => ({
                id: $(element).data("id"),
                name: $(element).data("name")
            })),
            rxjs.operators.filter(({ id }) => !subSystemIds.has(id))
        )
        .subscribe(subSystemStore.add);

    observeDescendantClick("#subsystems", "li[data-id] [data-action=remove]")
        .pipe(
            rxjs.operators.map(([element]) => $(element).closest("[data-id]").data("id"))
        )
        .subscribe(subSystemStore.remove);

    subSystemStore.subSystems$.pipe(rxjs.operators.map(subSystemsToHTML)).subscribe(html => $("#subsystems").html(html));
});

/**
 * Creates an observable that emits the value of an input.
 * @param {any} selector
 * @returns {import("../lib/rxjs/rxjs.umd").Observable}
 */
function observeInput(selector) {
    const element = $(selector);

    const value$ = new rxjs.BehaviorSubject(element.val());

    element.on("input", e => value$.next(e.target.value));

    return value$.asObservable();
}

/**
 * 
 * @param {any} selector
 * @param {any} descendantSelector
 * @returns {import("../lib/rxjs/rxjs.umd").Observable}
 */
function observeDescendantClick(selector, descendantSelector) {
    return new rxjs.Observable(subscriber => {
        $(selector).on("click", descendantSelector, function (e) {
            subscriber.next([this, e]);
        });

        return () => $(selector).off("click", descendantSelector);
    });
}

/**
 * Performs a search with the given term
 * @typedef {{id: string, name: string}} SearchResult
 * @param {string} term
 * @returns {Promise<SearchResult[]>}
 */
function search(term) {
    return fetch(`../products?searchterm=${term}`).then(response => response.json());
}

/**
 * Converts search results to HTML
 * @param {SearchResult[]} results
 */
function resultsToHTML(results) {
    return results.map(result => `<li data-id="${result.id}" class="bg-gray-100 p-4 mt-1 cursor-pointer hover:bg-gray-200" data-name="${result.name}">${result.name}</li>`).join("\n");
}

function createSubSystemStore() {
    const operations$ = new rxjs.BehaviorSubject(null);

    return {
        subSystems$: operations$.pipe(
            rxjs.operators.scan((subSystems, operation) => {
                if (operation == null)
                    return subSystems;

                switch (operation.type) {
                    case "add":
                        return [
                            ...subSystems,
                            { id: operation.id, name: operation.name }
                        ];
                    case "remove":
                        const index = subSystems.findIndex(subSystem => subSystem.id === operation.id);

                        return index >= 0 ? [
                            ...subSystems.slice(0, index),
                            ...subSystems.slice(index + 1)
                        ] : subSystems;
                    default:
                        return subSystems;
                }
            }, [])
        ),
        add({ id, name }) {
            operations$.next({ type: "add", id, name })
        },
        remove(id) {
            operations$.next({ type: "remove", id })
        }
    };
}

/**
 * Converts an array of subsystems into HTML
 * @param {any[]} subSystems
 * @returns {string}
 */
function subSystemsToHTML(subSystems) {
    return subSystems.map((subSystem, index) => {
        const removeButtonHTML = `<button type="button" class="bg-red-600 text-white mr-1 p-2 text-sm rounded-md hover:bg-red-700" data-action="remove">${subSystem.name}</button>`;

        return $(`<li class="list-none flex flex-row"><input name="SubSystems[${index}]" value="${subSystem.id}" hidden></li>`)
            .attr('data-id', subSystem.id)
            .attr('data-name', subSystem.name)
            .append(content)
            .append(removeButtonHTML)
            .prop("outerHTML");
    });
}