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
        if (!_.isEqual(prevProps.filters, this.props.filters)) {
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
        const response = await Promise.all(
            [
                fetch('api/results?' + new URLSearchParams(this.props.filters)),
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
            ..._.groupBy(results, r => r.possibleResponseName)
        };
        const sortGrouped = _.groupBy(results, r => r.executionTime);

        Object.keys(grouped).forEach(key => {
            // future if on what is across in table
            if (grouped[key].length !== executions.length) {
                executions.forEach(e => {
                    if (!Object.keys(sortGrouped).includes(e.occurredTime)) {
                        grouped[key].push({
                            executionTime: e.occurredTime
                        });
                    }
                });
            }

            grouped[key] = grouped[key].sort((a, b) => (a.executionTime < b.executionTime) ? -1 : ((a.executionTime > b.executionTime ? 1 : 0)));
        });

        const combinedSortGrouped = {
            ...Object.assign({}, ...executions.map(e => ({ [e.occurredTime]: [{ executionName: e.key }] }))),
            ...sortGrouped
        };
        const headers = [];

        Object.keys(combinedSortGrouped).sort((a, b) => (a.executionTime < b.executionTime) ? -1 : ((a.executionTime > b.executionTime ? 1 : 0))).forEach(key => {
            if (this.props.showQuestionNumber) {
                grouped['Question Number'].push({
                    count: combinedSortGrouped[key][0].questionNumber
                });
            }

            headers.push(combinedSortGrouped[key][0].executionName);
        });

        // This is a hack to force a sort order before a more official means.
        const forcedGrouped = _.merge({
            'Positive': [],
            'Neutral': [],
            'Negative': [],
            'Do Not Know/ No Basis to Judge': []
        }, grouped);

        this.setState({ results: forcedGrouped, headers: headers, loading: false });
    }
}
