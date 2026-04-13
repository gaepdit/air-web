function initMap() {
    // Initialize the map.
    const tiles = L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: 'Map data from <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>',
        detectRetina: true,
        className: 'map-tiles',
    });
    const map = L.map('facility-map', { center: [32.9, -83.3], zoom: 7, layers: [tiles] });

    // Fix the marker icon path.
    L.Icon.Default.prototype.options.imagePath = '/lib/leaflet/dist/images/';

    map.whenReady(function () {
        // Add a location control.
        L.control.locate({ initialZoomLevel: 13, onLocationError: showAlert }).addTo(map);
        map.on('locateactivate', removeAlert);

        // Enable permalinks.
        L.Permalink.setup(map);

        // Add markers for facilities with geocoordinates.
        const markers = L.markerClusterGroup({
            chunkedLoading: true,
            spiderLegPolylineOptions: { weight: 2, color: '#1d78c9', opacity: 0.5 },
        });
        for (const f of facilities) if (f.GeoCoordinates) markers.addLayer(newMarker(f));
        map.addLayer(markers);
    });
}

// Alert functions.
function removeAlert() {
    const alertElement = document.getElementById('map-warning');
    if (alertElement !== null) alertElement.remove();
}

function showAlert() {
    const alertElement = document.createElement('div');
    alertElement.innerText = 'Warning: Could not access location.';
    alertElement.classList.add('alert', 'alert-warning', 'alert-dismissible', 'fade', 'show');
    alertElement.setAttribute('id', 'map-warning');
    alertElement.setAttribute('role', 'alert');

    const closeButton = document.createElement('button');
    closeButton.dataset.bsDismiss = 'alert';
    closeButton.classList.add('btn-close');
    closeButton.setAttribute('type', 'button');
    closeButton.setAttribute('aria-label', 'Close');
    alertElement.append(closeButton);

    const mapElement = document.getElementById('facility-map');
    mapElement.parentNode.insertBefore(alertElement, mapElement);
}

// New marker function.
function newMarker(f) {
    const info = '<div class="facility-map-info">' +
        `<div class="facility-map-title">${f.Name}</div>` +
        `<div>${f.Location}</div>` +
        `<div class="facility-map-link"><a target="_blank" href="Details/${f.Id}">${f.Id}</a></div>` +
        '</div>';
    const marker = L.marker([f.GeoCoordinates.Latitude, f.GeoCoordinates.Longitude], { title: f.Name });
    marker.bindPopup(info);
    return marker;
}

initMap();
