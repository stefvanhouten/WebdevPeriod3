$(() => {
    $(".reply-button").on("click", function () {
        const commentForm = $(this).closest(".comment").find(".comment-form").first();
        $(".comment-form").not(commentForm).not(".top-level").addClass("hidden");
        commentForm.removeClass("hidden").find(".comment-input").focus();
    });
});