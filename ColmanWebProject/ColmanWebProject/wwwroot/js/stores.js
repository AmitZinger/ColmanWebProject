function loadMapScenario() {
    var map = new Microsoft.Maps.Map(document.getElementById('myMap'), {
        credentials: 'Ano2hnHZ1aJNouUlHCWXDpA7zEfag9ja4b1WM7EspvhozFKmUp4pZ35fGChaKfzu',
        mapTypeId: Microsoft.Maps.MapTypeId.road,
        zoom: 5
    });

    $.ajax({
        url: "/Products/SelectStores"
    }).done(function (data) {

    });

    // Create the infobox for the pushpin  
    var infobox = null;

    //declare addMarker function  
    function addMarker(latitude, longitude, title, description, pid) {
        var marker = new Microsoft.Maps.Pushpin(new Microsoft.Maps.Location(latitude, longitude), { color: 'green' });

        infobox = new Microsoft.Maps.Infobox(marker.getLocation(), {
            visible: false
        });

        marker.metadata = {
            id: pid,
            title: title,
            description: description
        };

        Microsoft.Maps.Events.addHandler(marker, 'mouseout', hideInfobox);
        Microsoft.Maps.Events.addHandler(marker, 'mouseover', showInfobox);

        infobox.setMap(map);
        map.entities.push(marker);
        marker.setOptions({ enableHoverStyle: true });
    };

    function showInfobox(e) {
        if (e.target.metadata) {
            infobox.setOptions({
                location: e.target.getLocation(),
                title: e.target.metadata.title,
                description: e.target.metadata.description,
                visible: true
            });
        }
    }

    function hideInfobox(e) {
        infobox.setOptions({ visible: false });
    }
}  