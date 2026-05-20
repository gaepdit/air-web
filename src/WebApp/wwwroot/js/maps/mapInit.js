function initMap() {
    // Initialize the map.
    const tiles = L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: 'Map data from <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>',
        detectRetina: true,
        className: 'map-tiles',
    });
    const map = L.map('single-facility-map', {
        center: [Facility.GeoCoordinates.Latitude, Facility.GeoCoordinates.Longitude],
        zoom: 14,
        layers: [tiles],
    });

    // Fix the marker icon path.
    L.Icon.Default.prototype.options.imagePath = '/lib/leaflet/dist/images/';

    map.whenReady(function () {
        // Add facility popup to map.
        const info = '<div class="facility-map-info">' +
            `<div class="facility-map-title">${Facility.Name}</div>` +
            `<div>${Facility.Location}</div>` +
            '</div>';
        L.popup([Facility.GeoCoordinates.Latitude, Facility.GeoCoordinates.Longitude], {
            closeOnEscapeKey: false,
            closeButton: false,
            closeOnClick: false,
        }).setContent(info).addTo(map);
    });
}

initMap();
