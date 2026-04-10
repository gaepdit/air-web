function initMap() {
    // Initialize the map.
    const tiles = L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: 'Map data from <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>',
        detectRetina: true,
        className: "map-tiles",
    });
    const map = L.map('facility-map', { center: [32.9, -83.3], zoom: 7, layers: [tiles] });

    // Fix the marker icon path.
    L.Icon.Default.prototype.options.imagePath = "/lib/leaflet/dist/images/"

    // Add a location control.
    L.control.locate({
        initialZoomLevel: 13,
        onLocationError: function () {
            const warningElement = document.createElement("div")
            warningElement.innerText = "Warning: Could not access location.";
            warningElement.classList.add("alert", "alert-warning");
            warningElement.setAttribute("id", "map-warning")
            warningElement.setAttribute("role", "alert");

            const mapElement = document.getElementById("facility-map");
            mapElement.parentNode.insertBefore(warningElement, mapElement);

            new bootstrap.Alert('#map-warning');
        }
    }).addTo(map);

    map.on("locateactivate", () => {
        const alert = bootstrap.Alert.getInstance('#map-warning');
        if (alert !== null) alert.close();
    });

    // Add facility markers.
    function makeMarker(f) {
        const info = '<div class="facility-map-info">' +
            `<div class="facility-map-title">${f.Name}</div>` +
            `<div>${f.Location}</div>` +
            `<div class="facility-map-link"><a target="_blank" href="Details/${f.Id}">${f.Id}</a></div>` +
            '</div>';
        const marker = L.marker([f.GeoCoordinates.Latitude, f.GeoCoordinates.Longitude], { title: f.Name });
        marker.bindPopup(info);
        markers.addLayer(marker);
    }

    const markers = L.markerClusterGroup({
        chunkedLoading: true,
        spiderLegPolylineOptions: { weight: 2, color: '#1d78c9', opacity: 0.5 }
    });
    facilities.flatMap((f) => f.GeoCoordinates ? makeMarker(f) : []);
    map.addLayer(markers);
}

initMap();
