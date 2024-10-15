// During the first two weeks of October, verify the selected FCE year before proceeding since during that period,
// FCEs are often created for both the current and previous year. 
document.addEventListener('DOMContentLoaded', function () {
    const form = document.querySelector('#new-fce-form');
    form.addEventListener('submit', function (event) {
        const today = new Date();
        if (today.getMonth() === 9 && today.getDate() <= 16) { // If today is within the first two weeks of October.
            const selectedYear = document.querySelector('select[name="Item.Year"]').value;
            const yearDescription = Number(selectedYear) === today.getFullYear() ? "recently ended September\u{00A0}30" : "just began October\u{00A0}1";
            const userConfirmed = confirm(`You have selected the year ${selectedYear}, the fiscal year that ${yearDescription}. Please confirm this is correct.`);
            if (!userConfirmed) event.preventDefault();
        }
    });
});
