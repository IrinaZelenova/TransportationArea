initMap();

async function initMap() {
    await ymaps3.ready;

    const { YMap, YMapDefaultSchemeLayer, YMapFeature, YMapDefaultFeaturesLayer, YMapControls } = ymaps3;
    const { YMapZoomControl } = await ymaps3.import('@yandex/ymaps3-controls@0.0.1');
   
    const FEATURE1 = {
        id: 'one',
        draggable: true,
        geometry: {
            type: 'Polygon',
            coordinates: [
                [
                    // Specify the coordinates of the vertices of the rectangle.
                    [37.6, 55.66],
                    [37.66, 55.66],
                    [37.66, 55.71],
                    [37.6, 55.71]
                ]
            ]
        },
        style: {
            fill: 'rgba(56, 56, 219, 0.5)',
            stroke: [{ color: '#f00', width: 4 }]
        }
    };

    

    const map = new YMap(
        document.getElementById('map'),
        {
            location: {
                center: [37.601, 55.674],
                zoom: 11
            }
        },
       );
    

    map.addChild(new YMapDefaultSchemeLayer());
    map.addChild(new YMapControls({ position: 'right' }).addChild(new YMapZoomControl({})));

    map.addChild(new YMapDefaultFeaturesLayer());
    map.addChild(new YMapFeature(FEATURE1));
   
}

