function setEAS(data, labels, labelData, labelsCount, pieData, noSchedule) {

    $('.dashboard-item .eas .btn-refresh').click(function () {
        var periodCont = $('.dashboard-item .eas .period-cont');
        var start = periodCont.attr('data-start-date').replaceAll('/', '-');
        var end = periodCont.attr('data-end-date').replaceAll('/', '-');

        EAS_applyPeriod(start, end);
    });

    if (noSchedule) return;

    var labelFormatter = function (label, series) {
        return "<div style='font-size:8pt; text-align:center; padding:2px; color:white;'>" + label + "<br/>" + Math.round(series.percent) + "%</div>";
    };

    var tooltip = $("<div class='dash_chart_1_tooltip chart-tooltip'></div>");

    var chart1 = $('#dash_chart_1');

    $.plot(chart1, data, {
        series: { lines: { show: true }, points: { show: true } },
        grid: { hoverable: true, clickable: true },
        xaxis: {
            max: labelsCount - 1, ticks: labels, mode: 'time'
        },
        yaxis: {
            tickDecimals: 1
        }
    });

    tooltip.clone().appendTo('body');

    chart1.bind('plothover', function (event, pos, item) {
        var tooltip = $('.dash_chart_1_tooltip');

        if (item) {
            var x = item.datapoint[0].toFixed(0),
                y = item.datapoint[1].toFixed(1);

            var o = null;
            labelData.some(function (data) {
                if (data[0] == x) {
                    o = data;
                    return true;
                } else {
                    return false;
                }
            });
            
            var _dt = o[1];
            var dt = moment(_dt).format('D MMM YYYY');

            tooltip.html(
                '<div class="tt-date">' + dt + '</div>' +
                '<div class="tt-series">' + item.series.label + ': ' + y + '</div>'
            )
                .css({ top: pos.pageY + 5, left: pos.pageX + 10 })
                .fadeIn(200);
        } else {
            tooltip.hide();
        }
    });


    $.plot('#dash_chart_2', pieData, {
        series: {
            pie: {
                show: true,
                radius: 1,
                label: {
                    show: true,
                    radius: 2 / 3,
                    formatter: labelFormatter,
                    threshold: 0.1
                }
            }
        },
        colors: ['#FF9806', '#e199b8', '#dcc8a3', '#a6b8ce', '#d36363'],
        legend: {
            show: true,
            noColumns: 1,
            container: $('.dashboard-item .eas .pie-legend')
        }
    });

   

}