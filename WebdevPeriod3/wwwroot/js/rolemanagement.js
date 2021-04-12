$(() => {
    $(".admin-checkbox").on("change", async function () {
        const userId = $(this).prop("disabled", true).data("user-id");
        const checked = $(this).prop("checked");

        try {
            const response = await fetch(`/api/role/${userId}/${checked ? "add" : "remove"}/Admin`, {
                method: "post"
            });

            if (!response.ok) $(this).prop("checked", !checked);
        } catch {
            $(this).prop("checked", !checked);
        } finally {
            $(this).prop("disabled", false);
        }
    })
});