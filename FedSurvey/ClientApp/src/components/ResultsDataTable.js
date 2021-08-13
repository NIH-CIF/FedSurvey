import React, { Component } from 'react';
import { Table } from 'reactstrap';
import _ from 'lodash';
import { CSVLink } from 'react-csv';
import api from '../api';

export class ResultsDataTable extends Component {
    static displayName = ResultsDataTable.name;

    constructor(props) {
        super(props);
        this.state = {
            results: [],
            headers: [],
            headerLastSort: {},
            loading: true
        };

        // I really do not like that this is here.
        this.CSV_NAMES = {
            dataGroupName: 'Organization',
            executionTime: 'Year',
            possibleResponseName: 'Response',
            questionText: 'Question'
        };
    }

    componentDidMount() {
        if (this.props.addButton && this.props.downloadable) {
            this.props.addButton({
                onClick: e => this.link.link.click(),
                text: 'Export'
            });
        }
    }

    componentDidUpdate(prevProps) {
        if (!_.isEqual(prevProps, this.props)) {
            this.setState({ loading: true });
            this.populateResultsData();
        }
    }

    iconForSortStatus(status) {
        if (status === 'asc') {
            return <i className="fas fa-sort-up"></i>;
        } else if (status === 'desc') {
            return <i className="fas fa-sort-down"></i>;
        } else {
            return <i className="fas fa-sort"></i>;
        }
    }

    render() {
        return !this.state.loading ? (
            <Table>
                <thead>
                    <tr>
                        <th></th>
                        {this.state.headers.map((h, i) => (
                            <th
                                key={i}
                                onClick={e => this.sortBy(i)}
                                style={this.props.sortable && { cursor: 'pointer' }}
                            >
                                <div style={{ display: 'flex', alignItems: 'center' }}>
                                    <span style={{ marginRight: 4 }}>
                                        {h}
                                    </span>
                                    {this.props.sortable && this.iconForSortStatus(this.state.headerLastSort[i])}
                                </div>
                            </th>
                        ))}
                        {this.props.downloadable && (
                            <th style={{ borderBottom: 'none', display: this.props.addButton ? 'none' : 'table-cell' }}>
                                <CSVLink data={this.getCsv()} filename="data.csv" ref={link => this.link = link}>
                                    <i className="fas fa-download"></i>
                                </CSVLink>
                            </th>
                        )}
                    </tr>
                </thead>
                <tbody>
                    {this.state.results.map(([key, val]) => (
                        <tr key={key}>
                            <th scope="row">{key}</th>
                            {val.map((r, index) => {
                                // undefined for r.count because it is not in object
                                // null for r.percentage because it is in object
                                return (r.count !== undefined || r.percentage !== undefined) ? (
                                    <td key={index}>{r.percentage?.toFixed(1) || r.count}{r.percentage !== null && r.percentage !== undefined && '%'}</td>
                                ) : <td key={index}>N/A</td>;
                            })}
                        </tr>
                    ))}
                </tbody>
            </Table>
        ) : 'Loading...';
    }

    getCsv() {
        return [[this.CSV_NAMES[this.props.groupingVariable], ...this.state.headers]].concat(this.state.results.map(([key, value]) => [key, ...value.map(v => v.percentage ? v.percentage + '%' : v.count)]));
    }

    sortBy(index, givenNewSort = null) {
        if (!this.props.sortable)
            return;

        const newSort = givenNewSort || (this.state.headerLastSort[index] === 'asc' ? 'desc' : 'asc');

        const sortedResults = this.state.results.sort(([ak, av], [bk, bv]) => {
            if (av[index] === undefined || (av[index].percentage === undefined && av[index].count === undefined)) {
                return 1;
            } else if (bv[index] === undefined || (bv[index].percentage === undefined && bv[index].count === undefined)) {
                return -1;
            } else {
                const ascSort = ((av[index].percentage !== null ? av[index].percentage : av[index].count) < (bv[index].percentage !== null ? bv[index].percentage : bv[index].count)) ? -1 : (((av[index].percentage !== null ? av[index].percentage : av[index].count) > (bv[index].percentage !== null ? bv[index].percentage : bv[index].count)) ? 1 : 0);

                if (newSort === 'desc') {
                    return ascSort * -1;
                } else {
                    return ascSort;
                }
            }
        });

        this.setState({ headerLastSort: { [index]: newSort }, results: sortedResults });
    }

    async populateResultsData() {
        // move elsewhere?
        Object.filter = (obj, predicate) =>
            Object.fromEntries(Object.entries(obj).filter(predicate));

        const searchPairs = [];

        Object.keys(Object.filter(this.props.filters, ([key, value]) => value.length > 0)).forEach(key => {
            this.props.filters[key].forEach(item => {
                searchPairs.push([key, item]);
            });
        });

        const response = await Promise.all(
            [
                api.fetch('api/results?' + new URLSearchParams(searchPairs)),
                api.fetch('api/executions') // later will be an if to be included based on config
            ]
        );
        const [results, executions] = await Promise.all(
            response.map(r => r.json())
        );

        const startingObject = this.props.showQuestionNumber ? {
            'Question Number': []
        } : {};

        const grouped = {
            ...startingObject,
            ..._.groupBy(results, r => r[this.props.groupingVariable])
        };
        const sortGrouped = _.groupBy(results, r => r[this.props.sortingVariable]);

        const showAllExecutions = this.props.sortingVariable === 'executionTime' && (this.props.filters['execution-keys'] === undefined || this.props.filters['execution-keys']?.length === 0);

        Object.keys(grouped).forEach(key => {
            if (showAllExecutions) {
                if (grouped[key].length !== executions.length) {
                    executions.forEach(e => {
                        if (!Object.keys(sortGrouped).includes(e.occurredTime)) {
                            grouped[key].push({
                                executionTime: e.occurredTime
                            });
                        }
                    });
                }
            }

            grouped[key] = grouped[key].sort((a, b) => (a[this.props.sortingVariable] < b[this.props.sortingVariable]) ? -1 : ((a[this.props.sortingVariable] > b[this.props.sortingVariable] ? 1 : 0)));
        });

        const executionKeys = showAllExecutions ? Object.assign({}, ...executions.map(e => ({ [e.occurredTime]: [{ executionName: e.key }] }))) : {};

        const combinedSortGrouped = {
            ...executionKeys,
            ...sortGrouped
        };
        const headers = [];

        Object.keys(combinedSortGrouped).sort((a, b) => (a < b) ? -1 : ((a > b ? 1 : 0))).forEach(key => {
            if (this.props.showQuestionNumber) {
                grouped['Question Number'].push({
                    count: combinedSortGrouped[key][0].questionNumber
                });
            }

            headers.push(this.props.sortingVariable === 'executionTime' ? combinedSortGrouped[key][0].executionName : combinedSortGrouped[key][0][this.props.sortingVariable]);
        });

        // This is a hack to force a sort order before a more official means.
        // Once supporting different question types, this must change.
        const forcedGrouped = _.merge({
            'Positive': [],
            'Neutral': [],
            'Negative': [],
            'Do Not Know/ No Basis to Judge': []
        }, grouped);

        for (const key in forcedGrouped) {
            if (forcedGrouped[key].length === 0) {
                delete forcedGrouped[key];
            } else if (showAllExecutions && forcedGrouped[key].length !== executions.length) {
                executions.forEach((e, index) => {
                    if (!forcedGrouped[key][index] || forcedGrouped[key][index].executionTime !== e.occurredTime) {
                        forcedGrouped[key].splice(index, 0, {
                            executionTime: e.occurredTime
                        });
                    }
                });
            } else if (forcedGrouped[key].length !== headers.length) {
                headers.forEach((h, index) => {
                    if (!forcedGrouped[key][index] || forcedGrouped[key][index][this.props.sortingVariable === 'executionTime' ? 'executionName' : this.props.sortingVariable] !== h) {
                        forcedGrouped[key].splice(index, 0, {
                            [this.props.sortingVariable]: h
                        });
                    }
                });
            }
        }

        const forcedGroupedEntries = Object.entries(forcedGrouped).map(([key, value]) => {
            if (this.props.groupingVariable === 'questionId') {
                const valueWithQuestionText = value.filter(v => v.questionText !== undefined);

                return [valueWithQuestionText[valueWithQuestionText.length - 1].questionText, value];
            } else {
                return [key, value];
            }
        });

        if (this.props.showDifference) {
            const newHeaders = [headers[0]].concat(headers.slice(1).map(h => ([h, 'Δ']))).flat();
            const newResults = forcedGroupedEntries.map(([key, value]) => {
                const newValue = value.map((v, index) => {
                    const prev = value[index - 1];

                    if (index === 0) {
                        return [v];
                    } else if (prev === undefined || prev.percentage === undefined || v.percentage === undefined) {
                        return [v, {}];
                    } else if (v.percentage === null) {
                        return [v, { count: v.count - prev.count }];
                    } else {
                        return [v, { percentage: v.percentage - prev.percentage }];
                    }
                }).flat();

                return [key, newValue];
            });

            this.setState({ results: newResults, headers: newHeaders, loading: false });
        } else {
            this.setState({ results: forcedGroupedEntries, headers: headers, loading: false });
        }

        // Bad and should only setState once.
        if (this.props.sort) {
            // allow index passed in too
            const index = this.props.sort.index || this.state.headers.indexOf(this.props.sort.header);

            this.sortBy(index, this.props.sort.direction);
        }
    }
}
