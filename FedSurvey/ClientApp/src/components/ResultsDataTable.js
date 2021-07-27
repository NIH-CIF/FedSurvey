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
                                return r ? (
                                    <td key={index}>{r.percentage?.toFixed(1) || r.count}{r.percentage && '%'}</td>
                                ) : <td></td>;
                            })}
                        </tr>
                    ))}
                </tbody>
            </Table>
        );
    }

    async populateResultsData() {
        const response = await fetch('api/results?' + new URLSearchParams(this.props.filters));
        const results = await response.json();

        const startingObject = this.props.showQuestionNumber ? {
            'Question Number': []
        } : {};

        const grouped = {
            ...startingObject,
            ..._.groupBy(results, r => r.possibleResponseName)
        };
        Object.keys(grouped).forEach(key => {
            grouped[key] = grouped[key].sort((a, b) => (a.executionTime < b.executionTime) ? -1 : ((a.executionTime > b.executionTime ? 1 : 0)));
        });

        const sortGrouped = _.groupBy(results, r => r.executionTime);
        const headers = [];

        // Open Q is force headers?

        Object.keys(sortGrouped).sort((a, b) => (a.executionTime < b.executionTime) ? -1 : ((a.executionTime > b.executionTime ? 1 : 0))).forEach(key => {
            if (this.props.showQuestionNumber) {
                grouped['Question Number'].push({
                    count: sortGrouped[key][0].questionNumber
                });
            }

            headers.push(sortGrouped[key][0].executionName);
        });

        this.setState({ results: grouped, headers: headers, loading: false });
    }
}
