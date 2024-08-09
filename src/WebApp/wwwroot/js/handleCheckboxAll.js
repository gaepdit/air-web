document.addEventListener('DOMContentLoaded', function () {
    const includeAllCheckbox = document.getElementById('Include_All');
    const includeOthersContainer = document.getElementById('Include_Others');

    includeAllCheckbox.addEventListener('change', function () {
        if (includeAllCheckbox.checked) {
            const checkboxes = includeOthersContainer.querySelectorAll('input[type="checkbox"]');
            checkboxes.forEach(function (checkbox) {
                checkbox.checked = false;
            });
        }
    });

    const otherCheckboxes = includeOthersContainer.querySelectorAll('input[type="checkbox"]');
    otherCheckboxes.forEach(function (checkbox) {
        checkbox.addEventListener('change', function () {
            if (checkbox.checked) {
                includeAllCheckbox.checked = false;
            }
        });
    });
});
