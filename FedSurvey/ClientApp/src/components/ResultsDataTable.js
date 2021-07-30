import React, { Component } from 'react';
import { Table } from 'reactstrap';
import { Input, Label } from 'reactstrap';
import { Link } from 'react-router-dom';
import _ from 'lodash';

export class ResultsDataTable extends Component {
    static displayName = ResultsDataTable.name;

    constructor(props) {
        super(props);
        this.state = {
            results: [], headers: [], loading: true };
    }

    componentDidMount() {
        this.populateResultsData();
    }

    componentDidUpdate(prevProps) {
        if (!_.isEqual(prevProps, this.props)) {
            this.populateResultsData();
        }
    }

    render() {

        return !this.state.loading && (
            <Table>
                <thead>
                    <tr>
                        <th></th>
                        {this.state.headers.map(h => (
                            <th key={h}>{h}</th>
                        ))}
                    </tr>
                </thead>
                <tbody>
                    {Object.entries(this.state.results).map(([key, val]) => (
                        <tr key={key}>
                            <th scope="row">{key}</th>
                            {val.map((r, index) => {
                                // undefined for r.count because it is not in object
                                // null for r.percentage because it is in object
                                return r.count !== undefined ? (
                                    <td key={index}>{r.percentage?.toFixed(1) || r.count}{r.percentage !== null && '%'}</td>
                                ) : <td key={index}>N/A</td>;
                            })}
                        </tr>
                    ))}
                </tbody>
            </Table>
        );
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
                fetch('api/results?' + new URLSearchParams(searchPairs)),
                fetch('api/executions') // later will be an if to be included based on config
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

        Object.keys(grouped).forEach(key => {
            if (this.props.sortingVariable === 'executionTime') {
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

        const executionKeys = this.props.sortingVariable === 'executionTime' ? Object.assign({}, ...executions.map(e => ({ [e.occurredTime]: [{ executionName: e.key }] }))) : {};

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
            } else if (this.props.sortingVariable === 'executionTime' && forcedGrouped[key].length !== executions.length) {
                executions.forEach((e, index) => {
                    if (!forcedGrouped[key][index] || forcedGrouped[key][index].executionTime !== e.occurredTime) {
                        forcedGrouped[key].splice(index, 0, {
                            executionTime: e.occurredTime
                        });
                    }
                });
            } else if (forcedGrouped[key].length !== headers.length) {
                headers.forEach((h, index) => {
                    if (!forcedGrouped[key][index] || forcedGrouped[key][index][this.props.sortingVariable] !== h) {
                        forcedGrouped[key].splice(index, 0, {
                            [this.props.sortingVariable]: h
                        });
                    }
                });
            }
        }

        this.setState({ results: forcedGrouped, headers: headers, loading: false });
    }
}
