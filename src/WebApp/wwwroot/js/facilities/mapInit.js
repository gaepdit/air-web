// Load The Google Maps libraries.
// https://developers.google.com/maps/documentation/javascript/load-maps-js-api#dynamic-library-import
(g => { var h, a, k, p = "The Google Maps JavaScript API", c = "google", l = "importLibrary", q = "__ib__", m = document, b = window; b = b[c] || (b[c] = {}); var d = b.maps || (b.maps = {}), r = new Set, e = new URLSearchParams, u = () => h || (h = new Promise(async (f, n) => { await (a = m.createElement("script")); e.set("libraries", [...r] + ""); for (k in g) e.set(k.replace(/[A-Z]/g, t => "_" + t[0].toLowerCase()), g[k]); e.set("callback", c + ".maps." + q); a.src = `https://maps.${c}apis.com/maps/api/js?` + e; d[q] = f; a.onerror = () => h = n(Error(p + " could not load.")); a.nonce = m.querySelector("script[nonce]")?.nonce || ""; m.head.append(a) })); d[l] ? console.warn(p + " only loads once. Ignoring:", g) : d[l] = (f, ...n) => r.add(f) && u().then(() => d[l](f, ...n)) })({
    key: apiKey, v: "weekly",
});

// Initialize the map.
async function initMap() {
    //  Request the needed libraries.
    const { Map, InfoWindow } = await google.maps.importLibrary("maps");
    const { AdvancedMarkerElement } = await google.maps.importLibrary("marker");
    const map = new google.maps.Map(document.getElementById("FacilityMap"), {
        zoom: 7,
        center: { lat: 32.9, lng: -83.3 },
        mapId: "FacilityMap",
    });

    // Add markers.
    const infoWindow = new google.maps.InfoWindow({
        content: "",
        maxWidth: 290,
    });
    const markers = facilities.map((f) => {
        const marker = new google.maps.marker.AdvancedMarkerElement({
            position: { lat: f.GeoCoordinates.Latitude, lng: f.GeoCoordinates.Longitude },
            title: `${f.Id} – ${f.Name}`
        });
        const header = document.createElement("div");
        header.innerHTML = `<div class="agm-title">${f.Name}</div>`;
        const info = '<div class="agm-info">' +
            `<div>${f.Location}</div>` +
            `<div class="agm-link"><a target="_blank" href="Details/${f.Id}">${f.Id}</a></div>` +
            '</div>';
        marker.addListener("gmp-click", () => {
            infoWindow.setValues({
                headerContent: header,
            });
            infoWindow.setContent(info);
            infoWindow.open(map, marker);
        });
        return marker;
    });

    // Add a marker clusterer to manage the markers.
    new markerClusterer.MarkerClusterer({ markers, map });
}
initMap();
