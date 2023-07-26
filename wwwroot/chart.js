var charts = {};
window.setup = (id,config) => {
    var ctx = document.getElementById(id).getContext('2d');
    if (typeof charts[id] !== 'undefined') { charts[id].destroy(); }

    if(!config.options.plugins){
        config.options.plugins = {};
    }
    config.options.plugins.legend = {
        onClick: (evt, legendItem, legend) => {
            let newVal = !legendItem.hidden;
            legend.chart.data.datasets.forEach(dataset => {
                if (dataset.label === legendItem.text) {
                    dataset.hidden = newVal
                }
            });
            legend.chart.update();
        },
        labels: {
            filter: (legendItem, chartData) => {
                let entries = chartData.datasets.map(e => e.label);
                return entries.indexOf(legendItem.text) === legendItem.datasetIndex;
            }
        }
    };
    config.options.plugins.tooltip = {
        displayColors: false,
        callbacks: {
            label: function (context){
                return [`Angle: ${context.dataset.slopeAngle}°`, `Distance: ${context.parsed.x}m`, `Elevation: ${context.parsed.y}m`];
            }
        }
    };
    config.options.plugins.zoom = {
        pan: {
            enabled: true,
            mode: 'x',
            modifierKey: 'ctrl',
        },
        zoom: {
            wheel: {
                enabled: true
            },
            drag: {
                enabled: true
            },
            pinch: {
                enabled: true
            },
            mode: 'x',
        },
    };
    
    charts[id] = new Chart(ctx, config);
};
window.resetChart = (id) => {
    if (typeof charts[id] !== 'undefined') { charts[id].resetZoom(); }
};