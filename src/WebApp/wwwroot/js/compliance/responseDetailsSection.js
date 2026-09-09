document.addEventListener('DOMContentLoaded', function () {
    const responseCheck = document.getElementById('Item_IsResponseReceived');
    const responseDetails = document.getElementById('responseDetails');

    if (!responseCheck || !responseDetails) return;

    responseCheck.addEventListener('change', function () {
        if (this.checked) {
            responseDetails.classList.remove('d-none');
        } else {
            responseDetails.classList.add('d-none');
        }
    })
});
