var charts = {};
window.setup = (id,config, instance) => {
    var ctx = document.getElementById(id).getContext('2d');
    if (typeof charts[id] !== 'undefined') { charts[id].destroy(); }

    if(!config.options.plugins){
        config.options.plugins = {};
    }
    
    config.options.plugins.legend = {
        onClick: (evt, legendItem, legend) => {
            console.log(legendItem)
            let newVal = !legendItem.hidden;
            legend.chart.data.datasets.forEach(dataset => {
                if (dataset.label === legendItem.text) {
                    console.log(dataset)
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
                return context.raw.s.map(x => x); 
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
                // let xTicks = chart.scales.xAxes.ticks;
                // let distance = 0;
                // if(xTicks.length >= 2){
                //     distance = Math.max(0, xTicks[xTicks.length - 1].value - xTicks[0].value)
                // }
                //
                // distance = Math.min(chart.config.data.maxTotalDistance, distance)

                const start = chart.scales.xAxes.start;
                const end = chart.scales.xAxes.end;
                await instance.invokeMethodAsync('ZoomChanged', start, end);
            }
        },
    };
    
    var chart = new Chart(ctx, config);
    chart.instance = instance;
    charts[id] = chart;
};
window.resetChart = async (id) => {
    if (typeof charts[id] !== 'undefined') { 
        charts[id].resetZoom();
        await charts[id].instance.invokeMethodAsync('ZoomReset');
    }
};