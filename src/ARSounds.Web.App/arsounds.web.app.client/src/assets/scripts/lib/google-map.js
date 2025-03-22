$(document).ready(function () {
    if (document.body.contains(document.getElementById('map'))) {
        var mapElement = new google.maps.Map(document.getElementById('map'), {
            center: {
                lat: -34.397,
                lng: 150.644
            },
            zoom: 8
        });
        var marker = new google.maps.Marker({
            map: mapElement,
            anchorPoint: new google.maps.Point(0, -29)
        });
    }
    if (document.body.contains(document.getElementById('FullAddress'))) {
        var autocomplete = new google.maps.places.Autocomplete(
            (document.getElementById('FullAddress')), {
            types: ['geocode']
        });
        var componentForm = {
            street_number: 'short_name',
            route: 'long_name',
            locality: 'long_name',
            administrative_area_level_1: 'short_name',
            country: 'long_name',
            postal_code: 'short_name'
        };
        autocomplete.addListener('place_changed', function () { fillInAddress.call(autocomplete, mapElement, autocomplete, marker, componentForm) });
        $("#FullAddress").focus(function () { geolocate.call(focus, autocomplete) });
    }
});

function fillInAddress(mapElement, autocomplete, marker, componentForm) {
    var place = autocomplete.getPlace();
    if (mapElement != undefined) {
        if (place.geometry.viewport) {
            mapElement.fitBounds(place.geometry.viewport);
        } else {
            mapElement.setCenter(place.geometry.location);
            mapElement.setZoom(17);
        }
        marker.setMap(null);
        marker.setOptions({
            position: place.geometry.location,
            map: mapElement
        });
    }

    $('#Latitude').val(place.geometry.location.lat());
    $('#Longitude').val(place.geometry.location.lng());

    for (var component in componentForm) {
        $('input[google-id="' + component + '"]').val('');
    }

    for (var i = 0; i < place.address_components.length; i++) {
        var addressType = place.address_components[i].types[0];
        if (componentForm[addressType]) {
            var val = place.address_components[i][componentForm[addressType]];
            $('input[google-id="' + addressType + '"]').val(val);
            $('span[data-valmsg-for="' + $('input[google-id="' + addressType + '"]').attr('id') + '"]').remove();
        }
    }
}

function geolocate(autocomplete) {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(function (position) {
            var geolocation = {
                lat: position.coords.latitude,
                lng: position.coords.longitude
            };
            var circle = new google.maps.Circle({
                center: geolocation,
              radius: position.coords.accuracy
            });
            autocomplete.setBounds(circle.getBounds());
        });
    }
}
