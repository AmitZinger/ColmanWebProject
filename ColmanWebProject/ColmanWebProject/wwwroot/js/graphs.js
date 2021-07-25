$(document).ready(function () {
    if (window.location.pathname === "/Products/ManageProducts") {
        $.ajax({
            url: "/Products/MostOrdered"
        }).done(function (data) {
            const svg = d3.select('#container').append("svg");

            const margin = 80;
            const width = 1000 - 2 * margin;
            const height = 600 - 2 * margin;

            const chart = svg.append('g')
                .attr('transform', `translate(${margin}, ${margin})`);

            const xScale = d3.scaleBand()
                .range([0, width])
                .domain(data.map((s) => s.name))
                .padding(0.4)

            const values = data.map(a => a.value);
            const yDomainMax = Math.max(...values) + Math.min(...values);

            const yScale = d3.scaleLinear()
                .range([height, 0])
                .domain([0, yDomainMax]);

            const makeYLines = () => d3.axisLeft()
                .scale(yScale)

            chart.append('g')
                .attr('transform', `translate(0, ${height})`)
                .call(d3.axisBottom(xScale));

            chart.append('g')
                .call(d3.axisLeft(yScale));

            chart.append('g')
                .attr('class', 'grid')
                .call(makeYLines()
                    .tickSize(-width, 0, 0)
                    .tickFormat('')
                )

            const barGroups = chart.selectAll()
                .data(data)
                .enter()
                .append('g')

            barGroups
                .append('rect')
                .attr('class', 'bar')
                .attr('x', (g) => xScale(g.name))
                .attr('y', (g) => yScale(g.value))
                .attr('height', (g) => height - yScale(g.value))
                .attr('width', xScale.bandwidth())
                .on('mouseenter', function (actual, i) {
                    d3.selectAll('.value')
                        .attr('opacity', 0)

                    d3.select(this)
                        .transition()
                        .duration(300)
                        .attr('opacity', 0.6)
                        .attr('x', (a) => xScale(a.name) - 5)
                        .attr('width', xScale.bandwidth() + 10)

                    const y = yScale(actual.value)

                    line = chart.append('line')
                        .attr('id', 'limit')
                        .attr('x1', 0)
                        .attr('y1', y)
                        .attr('x2', width)
                        .attr('y2', y)

                    barGroups.append('text')
                        .attr('class', 'divergence')
                        .attr('x', (a) => xScale(a.name) + xScale.bandwidth() / 2)
                        .attr('y', (a) => yScale(a.value) + 30)
                        .attr('fill', 'white')
                        .attr('text-anchor', 'middle')
                        .text((a, idx) => {
                            const divergence = (a.value - actual.value).toFixed(1)

                            let text = ''
                            if (divergence > 0) text += '+'
                            text += `${divergence}`

                            return idx !== i ? text : '';
                        })

                })
                .on('mouseleave', function () {
                    d3.selectAll('.value')
                        .attr('opacity', 1)

                    d3.select(this)
                        .transition()
                        .duration(300)
                        .attr('opacity', 1)
                        .attr('x', (a) => xScale(a.name))
                        .attr('width', xScale.bandwidth())

                    chart.selectAll('#limit').remove()
                    chart.selectAll('.divergence').remove()
                })

            barGroups
                .append('text')
                .attr('class', 'value')
                .attr('x', (a) => xScale(a.name) + xScale.bandwidth() / 2)
                .attr('y', (a) => yScale(a.value) + 30)
                .attr('text-anchor', 'middle')
                .text((a) => `${a.value}`)

            svg
                .append('text')
                .attr('class', 'label')
                .attr('x', -(height / 2) - margin)
                .attr('y', margin / 2.4)
                .attr('transform', 'rotate(-90)')
                .attr('text-anchor', 'middle')
                .text('Orders')

            svg.append('text')
                .attr('class', 'label')
                .attr('x', width / 2 + margin)
                .attr('y', height + margin * 1.7)
                .attr('text-anchor', 'middle')
                .text('Products')

            svg.append('text')
                .attr('class', 'title')
                .attr('x', width / 2 + margin)
                .attr('y', 40)
                .attr('text-anchor', 'middle')
                .text('Most Ordered Products')

            svg.append('text')
                .attr('class', 'source')
                .attr('x', width - margin / 2)
                .attr('y', height + margin * 1.7)
                .attr('text-anchor', 'start')
                .text('Per: ' + new Date().toLocaleDateString())
        })
    }

    if (window.location.pathname === "/Orders") {
        $.ajax({
            url: "/Orders/OrdersPricesPerMonth"
        }).done(function (data) {
            const svg = d3.select('#container').append("svg");

            const margin = 80;
            const width = 1000 - 2 * margin;
            const height = 600 - 2 * margin;

            const chart = svg.append('g')
                .attr('transform', `translate(${margin}, ${margin})`);

            const xScale = d3.scaleBand()
                .range([0, width])
                .domain(data.map((s) => s.date))
                .padding(0.4)

            const values = data.map(a => a.price);
            const yDomainMax = Math.max(...values) + Math.min(...values);

            const yScale = d3.scaleLinear()
                .range([height, 0])
                .domain([0, yDomainMax]);

            const makeYLines = () => d3.axisLeft()
                .scale(yScale)

            chart.append('g')
                .attr('transform', `translate(0, ${height})`)
                .call(d3.axisBottom(xScale));

            chart.append('g')
                .call(d3.axisLeft(yScale));

            chart.append('g')
                .attr('class', 'grid')
                .call(makeYLines()
                    .tickSize(-width, 0, 0)
                    .tickFormat('')
                )

            const barGroups = chart.selectAll()
                .data(data)
                .enter()
                .append('g')

            barGroups
                .append('rect')
                .attr('class', 'bar')
                .attr('x', (g) => xScale(g.date))
                .attr('y', (g) => yScale(g.price))
                .attr('height', (g) => height - yScale(g.price))
                .attr('width', xScale.bandwidth())
                .on('mouseenter', function (actual, i) {
                    d3.selectAll('.value')
                        .attr('opacity', 0)

                    d3.select(this)
                        .transition()
                        .duration(300)
                        .attr('opacity', 0.6)
                        .attr('x', (a) => xScale(a.date) - 5)
                        .attr('width', xScale.bandwidth() + 10)

                    const y = yScale(actual.price)

                    line = chart.append('line')
                        .attr('id', 'limit')
                        .attr('x1', 0)
                        .attr('y1', y)
                        .attr('x2', width)
                        .attr('y2', y)

                    barGroups.append('text')
                        .attr('class', 'divergence')
                        .attr('x', (a) => xScale(a.date) + xScale.bandwidth() / 2)
                        .attr('y', (a) => yScale(a.price) + 30)
                        .attr('fill', 'white')
                        .attr('text-anchor', 'middle')
                        .text((a, idx) => {
                            const divergence = (a.price - actual.price).toFixed(1)

                            let text = ''
                            if (divergence > 0) text += '+'
                            text += `${divergence}`

                            return idx !== i ? text : '';
                        })

                })
                .on('mouseleave', function () {
                    d3.selectAll('.value')
                        .attr('opacity', 1)

                    d3.select(this)
                        .transition()
                        .duration(300)
                        .attr('opacity', 1)
                        .attr('x', (a) => xScale(a.date))
                        .attr('width', xScale.bandwidth())

                    chart.selectAll('#limit').remove()
                    chart.selectAll('.divergence').remove()
                })

            barGroups
                .append('text')
                .attr('class', 'value')
                .attr('x', (a) => xScale(a.date) + xScale.bandwidth() / 2)
                .attr('y', (a) => yScale(a.price) + 30)
                .attr('text-anchor', 'middle')
                .text((a) => `${a.price}`)

            svg
                .append('text')
                .attr('class', 'label')
                .attr('x', -(height / 2) - margin)
                .attr('y', margin / 2.4)
                .attr('transform', 'rotate(-90)')
                .attr('text-anchor', 'middle')
                .text('Prices')

            svg.append('text')
                .attr('class', 'label')
                .attr('x', width / 2 + margin)
                .attr('y', height + margin * 1.7)
                .attr('text-anchor', 'middle')
                .text('Dates')

            svg.append('text')
                .attr('class', 'title')
                .attr('x', width / 2 + margin)
                .attr('y', 40)
                .attr('text-anchor', 'middle')
                .text('Average Order Prices Per Month')

            svg.append('text')
                .attr('class', 'source')
                .attr('x', width - margin / 2)
                .attr('y', height + margin * 1.7)
                .attr('text-anchor', 'start')
                .text('Per: ' + new Date().toLocaleDateString())
        })
    }
})
