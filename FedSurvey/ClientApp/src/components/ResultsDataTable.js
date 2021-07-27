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
            results: [], loading: true };
    }

    componentDidMount() {
        this.populateResultsData();
    }

    render() {
        const headers = this.getHeaders();

        return !this.state.loading && (
            <Table>
                <thead>
                    <tr>
                        <th></th>
                        {headers.map(h => (
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

    getHeaders() {
        return Object.keys(this.state.results).length > 0 ? this.state.results[Object.keys(this.state.results)[0]].map(g => g.executionName) : {};
    }

    async populateResultsData() {
        const response = await fetch('api/results?' + new URLSearchParams({ 'question-ids': [48], 'data-group-names': ['Dummy'] }));
        const results = await response.json();

        const grouped = _.groupBy(results, r => r.possibleResponseName);
        Object.keys(grouped).forEach(key => {
            grouped[key] = grouped[key].sort((a, b) => (a.executionTime < b.executionTime) ? -1 : ((a.executionTime > b.executionTime ? 1 : 0)));
        });

        const sortGrouped = _.groupBy(results, r => r.executionTime);
        grouped['Question Number'] = [];
        Object.keys(sortGrouped).sort((a, b) => (a.executionTime < b.executionTime) ? -1 : ((a.executionTime > b.executionTime ? 1 : 0))).forEach(key => {
            grouped['Question Number'].push({
                count: sortGrouped[key][0].questionNumber
            });
        });

        this.setState({ results: grouped, loading: false });
    }
}
