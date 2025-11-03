// Don't submit empty form fields.
// (Add this script to search forms that use GET for submit. It keeps clutter out of the resulting query string.)
$(document).ready(function () {
    function disableEmptyInput(n, el) {
        const $input = $(el);
        if ($input.val() === '')
            $input.attr('disabled', 'disabled');
    }

    $('#SearchButton').click(function DisableEmptyInputs(e) {
        const $form = $(this).closest('form');
        // Check if the jQuery validator exists
        if ($form.data('validator') || typeof $form.validate === "function") {
            // If validator is present, check if the form is valid
            if (!$form.valid()) {
                // Prevent disabling inputs and form submission
                e.preventDefault();
                return false;
            }
        }
        // Only disable inputs if form is valid (or if no validator)
        $('input').each(disableEmptyInput);
        $('select').each(disableEmptyInput);
        return true;
    });
});
