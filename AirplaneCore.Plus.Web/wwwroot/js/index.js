let lang = '';
const styles = ['basic', 'streets', 'bright', 'light', 'dark', 'satellite'];

(function () {
    if (navigator.languages) {
        var langs = navigator.languages;
        lang = langs[0];
    }
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(renermap);
    }
})();

function renermap(loc) {
    //let style = Math.floor(Math.random() * (max - min + 1)) + min;
    let style = Math.floor(Math.random() *6);
    redraw(loc, style === undefined || isNaN(style) ? 0 : style);
}

function redraw(loc, style) {
    mapboxgl.accessToken = document.querySelector('#mapboxtoken').value;

    let map = new mapboxgl.Map({
        container: 'map',
        style: 'mapbox://styles/mapbox/' + styles[style] + '-v9',
        center: [loc.coords.longitude, loc.coords.latitude],
        zoom: 7
    });

    let nav = new mapboxgl.NavigationControl();
    map.addControl(nav, 'bottom-right');

    map.on('load', () => {
        mapcallback(map);
    });
}

function mapcallback(map) {
    map.addSource('airports',
        {
            type: 'geojson',
            //data: 'https://localhost:9801/api/airports/get',
            data: '/api/airports/get',
            //cluster: true, // Enable clustering
            //clusterRadius: 50, // Radius of each cluster when clustering points
            //clusterMaxZoom: 6 // Max zoom to cluster points on
        });

    map.addLayer({
        id: 'airport',
        type: 'circle',
        source: 'airports',
        filter: ['!has', 'point_count'],
        paint: {
            //'circle-color': {
            //    property: 'point_count',
            //    type: 'interval',
            //    stops: [
            //        [0, '#41A337'],
            //        [100, '#2D7026'],
            //        [750, '#0B5703']
            //    ]
            //},
            'circle-color': 'navy',
            'circle-radius': 6,
            //'circle-radius': {
            //    property: 'point_count',
            //    type: 'interval',
            //    stops: [
            //        [0, 20],
            //        [100, 30],
            //        [750, 40]
            //    ]
            //},
            'circle-stroke-width': 1,
            'circle-stroke-color': '#fff'
        }
    });

    //var popup = new mapboxgl.Popup({
    //    closeButton: false,
    //    closeOnClick: false
    //});

    map.on('mouseenter', 'airport', (e) => {
        // Change the cursor style as a UI indicator.
        map.getCanvas().style.cursor = 'pointer';

        // Populate the popup and set its coordinates based on the feature found.
        let coordinates = e.features[0].geometry.coordinates.slice();
        let name = e.features[0].properties.name;
        let iataCode = e.features[0].properties.iataCode;
        let showIataCode = iataCode && iataCode.length > 0 ? ` | ${iataCode}` : '';

        //popup.setLngLat(coordinates)
        //    .setHTML(`<strong>${name}${showIataCode}</strong>`)
        //    .addTo(map);

        document.querySelector('#airport-name').innerText = `${name}${showIataCode}`;
        document.querySelector('#info-card').style.display = '';

        let lng = coordinates[0];
        let lat = coordinates[1];
        fetch(`/api/g/r/${lng}/${lat}/${name}/400/${lang}`)
            .then(blob => blob.json())
            .then(data => {
                // Set airport properties
                if (data.photo)
                    document.querySelector('#airport-image').src = 'data:image/png;base64,' + data.photo;
                else
                    document.querySelector('#airport-image').src = 'https://via.placeholder.com/400x200?text=No+Image+Found';

                document.querySelector('#airport-name').innerText = `${data.name}${showIataCode}`;
                document.querySelector('#airport-address').innerText = data.formattedAddress || '';
                document.querySelector('#airport-phone').innerText = data.phoneNumber || '';
                document.querySelector('#airport-website').innerText = data.website || '';

                // display more info
                document.querySelector('#more-info').style.display = '';
            })
            .catch(error => {
                console.log(error);
                document.querySelector('#airport-image').src = 'https://via.placeholder.com/400x200?text=Error+while+loading+data';
            });
    });

    map.on('mouseleave', 'airport', () => {
        map.getCanvas().style.cursor = '';
        //popup.remove();
        document.querySelector('#info-card').style.display = 'none';

        document.querySelector('#airport-image').src = 'https://via.placeholder.com/400x200?text=loading';
        document.querySelector('#airport-name').innerText = '';
        document.querySelector('#airport-address').innerText = '';
        document.querySelector('#airport-phone').innerText = '';
        document.querySelector('#airport-website').innerText = '';
    });

    var placesAutocomplete = places({
        container: document.querySelector('#place-input'),
        type: 'city'
    });

    placesAutocomplete.on('change', e => {
        map.flyTo({
            center: [e.suggestion.latlng.lng, e.suggestion.latlng.lat],
            zoom: 9
        });
    });
}

document.querySelector('#info-card-close-button').addEventListener('click', function (event) {
    document.querySelector('#info-card').style.display = 'none';

    document.querySelector('#airport-image').src = 'https://via.placeholder.com/400x200?text=loading';
    document.querySelector('#airport-name').innerText = '';
    document.querySelector('#airport-address').innerText = '';
    document.querySelector('#airport-phone').innerText = '';
    document.querySelector('#airport-website').innerText = '';
});

//document.querySelector('select[id="mapStyleChanger"]').onchange = mapStyleChangerHandler;
//function mapStyleChangerHandler(e) {
//    let val = parseInt(e.target.value);
//    if (isNaN(val)) {
//        val = 0;
//    }
//    if (navigator.geolocation) {
//        navigator.geolocation.getCurrentPosition(renermap(val));
//    }
//}