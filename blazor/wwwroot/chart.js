var charts = {};
window.setup = (id,config, instance) => {
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
                return [`${context.dataset.slopeType}: ${context.dataset.slopeAngle}${context.dataset.slopeTypeSuffix}`, `Distance: ${context.parsed.x}m`, `Elevation: ${context.parsed.y}m`];
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
            onZoomComplete: async function({chart}){
                
                const start = chart.scales.xAxes.start;
                const end = chart.scales.xAxes.end;
 
                let elevation = 0.0;
                const datasets = chart.config.data.datasets;
                for (let i = 0; i < datasets.length; i++){
                    const data = datasets[i].data
                    if(data[0].x < start){
                        continue;
                    }
                    
                    let previous = data[0];
                    for (let j = 1; j < data.length; j++){
                        if(data[j].x > start && data[j].x < end){
                            elevation += Math.max(0, data[j].y - previous.y) 
                        }
                        
                        previous = data[j];
                    }
                }

                let xTicks = chart.scales.xAxes.ticks;
                let distance = 0;
                if(xTicks.length >= 2){
                    distance = Math.max(0, xTicks[xTicks.length - 1].value - xTicks[0].value)
                }
                
                distance = Math.min(chart.config.data.maxTotalDistance, distance)

                await instance.invokeMethodAsync('ZoomChanged', elevation, distance);
            }
        },
    };
    
    charts[id] = new Chart(ctx, config);
};
window.resetChart = (id) => {
    if (typeof charts[id] !== 'undefined') { charts[id].resetZoom(); }
};