document.addEventListener('DOMContentLoaded', () => {
    const multidaySwitch = document.getElementById('Item_MultiDayInspection');
    if (!multidaySwitch) return;

    multidaySwitch.addEventListener('change', () => {
        const endTimeElement = document.getElementById('Item_InspectionEndedTime').parentElement;
        const multiOffSection = document.getElementById('date-multi-off');
        const multiOnSection = document.getElementById('date-multi-on');

        if (!endTimeElement || !multiOffSection || !multiOnSection) return;

        if (multidaySwitch.checked) {
            multiOnSection.appendChild(endTimeElement);
            multiOnSection.classList.remove('d-none');
            multiOffSection.classList.add('col-md');
        } else {
            multiOffSection.appendChild(endTimeElement);
            multiOnSection.classList.add('d-none');
            multiOffSection.classList.remove('col-md');
        }
    });
});
