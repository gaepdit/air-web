function initMap() {
    // Initialize the map.
    const tiles = L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: 'Map data from <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>',
        detectRetina: true,
        className: "map-tiles",
    });
    const map = L.map('FacilityMap', { center: [32.9, -83.3], zoom: 7, layers: [tiles] });

    // Fix the marker icon path.
    L.Icon.Default.prototype.options.imagePath = "/lib/leaflet/dist/images/"

    // Add a locate control.
    L.control.locate({ initialZoomLevel: 13 }).addTo(map);

    // Mark the current location when found.
    // Star icon created by Pixel perfect - Flaticon: https://www.flaticon.com/free-icons/star
    const star = L.icon({ iconUrl: '/images/star.png', iconSize: [24, 24] });
    map.on('locatelocationfound', function (e) {
        L.marker(e.latlng, { title: "You are here", icon: star }).addTo(map);
    });

    // Add markers.
    function makeMarker(f) {
        const info = '<div class="facility-map-info">' +
            `<div class="facility-map-title">${f.Name}</div>` +
            `<div>${f.Location}</div>` +
            `<div class="facility-map-link"><a target="_blank" href="Details/${f.Id}">${f.Id}</a></div>` +
            '</div>';
        const marker = L.marker([f.GeoCoordinates.Latitude, f.GeoCoordinates.Longitude], {
            title: f.Name,
            riseOnHover: true,
        });
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
