
$(function () {
    initMap();
})
var app = {
    mymap: null
};
function initMap() {
    if (document.getElementById('searchmap') != null) {
        L.HtmlIcon = L.Icon.extend({
            options: {
                /*
                html: (String) (required)
                iconAnchor: (Point)
                popupAnchor: (Point)
                */
            },

            initialize: function (options) {
                L.Util.setOptions(this, options);
            },

            createIcon: function () {
                var div = document.createElement('div');
                div.innerHTML = this.options.html;
                if (div.classList)
                    div.classList.add('leaflet-marker-icon');
                else
                    div.className += ' ' + 'leaflet-marker-icon';
                return div;
            },

            createShadow: function () {
                return null;
            }
        });

        var tiles = L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
            maxZoom: 18,
            attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors, Points &copy 2012 LINZ'
        });
        if ($('#lat').val() && $('#lng').val()) {
            latlng = L.latLng($('#lat').val() , $('#lng').val());
        }
        else {
            latlng = L.latLng(-37.82, 175.24);
        }

        app.mymap = L.map('searchmap', { center: latlng, zoom: 13, layers: [tiles] });
        RenderCompanyLocs();
        //var markers = L.markerClusterGroup();
        //var k = 1;
        //for (var i = 0; i < addressPoints.length; i++) {
        //    var a = addressPoints[i];

        //    if (k == 30) {
        //        k = 1;
        //    }

        //    var markerHTML = new L.HtmlIcon({
        //        html: "<img class='leaflet-marker-icon leaflet-zoom-animated leaflet-interactive' src='images/marker/" + k + ".png' alt='markericon' />",
        //    });

        //    markers.on('clusterclick', function () {
        //        k = 1;
        //        var markerHTML = new L.HtmlIcon({
        //            html: "<img class='leaflet-marker-icon leaflet-zoom-animated leaflet-interactive' src='images/marker/" + k + ".png' alt='markericon' />",
        //        });
        //    });


        //    var title = a[2];
        //    var marker = L.marker(new L.LatLng(a[0], a[1]), { icon: markerHTML });
        //    marker.bindPopup(title, { offset: new L.Point(0, -170) });
        //    markers.addLayer(marker);
        //    k++;
        //}

        //map.addLayer(markers);
    }
}
function RenderCompanyLocs() {
    for (var i = 0; i < companyLocations.length; i++) {
        var item = companyLocations[i];
        addmarker(item);

    }
}

var mymarker;
function addmarker(item) {

    var marker = L.marker(new L.LatLng(item.lat, item.lng), {
        icon: new L.DivIcon({
            className: 'my-div-icon',
            html: '<img class="my-div-image" src="/img/marker.png"/>'

        })
    }).addTo(app.mymap);
    marker.customData = item;
  

//    marker.addTo(app.markergroup);
    marker.on('click', onMapClick);
    app.mymap.setZoom(12);

}
function onMapClick(e) {
    mymarker = e;

    var popup = L.popup();

    var item = e.target.customData
    popup
        .setLatLng(e.latlng)

        .setContent("<div style='text-align:left !important;padding: 20px;'><b>Company: </b> " + item.name + "<br/><b></b>" + item.address +  "</div>")
        .openOn(app.mymap);
}
