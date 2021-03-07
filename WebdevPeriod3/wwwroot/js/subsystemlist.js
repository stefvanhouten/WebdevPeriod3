'use strict';

function getButtonRow(buttonEl) { 
  buttonEl.closest('input'); 
}

function debounce(callback, wait, immediate = false) {
  let timeout = null 
  
  return function() {
    const callNow = immediate && !timeout
    const next = () => callback.apply(this, arguments)
    
    clearTimeout(timeout)
    timeout = setTimeout(next, wait)

    if (callNow) {
      next()
    }
  }
}

let ctrl = null;

async function search(term) {
  if (ctrl) ctrl.abort();
  ctrl = new AbortController();

  const products = await fetch(`../products?searchterm=${term}`, {
    signal: ctrl.signal
  })
    .then((res) => res.json())
    .catch((ex) => (ex.name === "AbortError" ? null : Promise.reject(ex)))
    .finally(() => {
      ctrl = null;
    });

    console.log(products);

  return products?.filter((product) =>
    product.name.toLowerCase()
  );
}

document.addEventListener("DOMContentLoaded", () => {

  let terms;

    document.getElementById("search-input").addEventListener("input", debounce(async (e) => {
    if (e.target.value)
		  terms = await search(e.target.value);
		if (terms) {
			document.getElementById("search-output").innerHTML = terms.slice(0,5).filter((term) => term.name != e.target.value).map((term) => `<li>${term.name}</li>`).join("");
		}
	}, 1000));

  [].forEach.call(document.getElementById("search-output").children, (element) => { 
    element.addEventListener('click', () => {
      document.getElementById("search-input").value = this.value;
    });
  });

  document.getElementById("search-output").addEventListener("click", (e) => {
    document.getElementById("search-input").value = e.target.innerHTML;
  })

  document.getElementById("add-system").addEventListener("click", () => {

    let subSystemName = document.getElementById("search-input").value;

    if (!terms || !terms.some(term => term.name === subSystemName))
      return;

    let chosenSubSystem = terms[terms.findIndex(term => term.name === subSystemName)];

    let SubsystemsEl = document.getElementById("SubsystemList");

    let formdiv = document.createElement("div");

    let subSystem = document.createElement("input");
    subSystem.className = "border p-2 w-full mt-3";
    subSystem.name = "Subsystems";
    subSystem.type = "hidden"
    subSystem.value = chosenSubSystem.id;
    subSystem.readOnly = true;

    let displayedSystem = subSystem.cloneNode(false)
    displayedSystem.name = "SubsystemName"
    displayedSystem.type = "text"
    displayedSystem.value = subSystemName;

    let removeBtn = document.createElement("button");
    removeBtn.className = "flex bg-red-600 text-white font-semibold border";
    removeBtn.id = "removeBtn";
    removeBtn.textContent = "DEL";
    removeBtn.type = "button";

    formdiv.append(subSystem, displayedSystem, removeBtn);

    SubsystemsEl.insertBefore(formdiv, SubsystemsEl.childNodes[0]);

    document.getElementById("search-input").value = "";
    document.getElementById("search-output").innerHTML = "";
  });

  document.getElementById("SubsystemList").addEventListener("click", (e) => {
    if (e.target && e.target.id === "removeBtn") {
      e.target.previousElementSibling.remove();
      e.target.previousElementSibling.remove();
      e.target.remove();
    }
  });
  

})