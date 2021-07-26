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
        return !this.state.loading && (
            <Table>
                <thead>
                    <tr>
                        <th></th>
                        {/*this.state.executions.map(e => (
                            <th key={e.id}>{e.key}</th>
                        ))*/}
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <th scope="row">Question Number</th>
                        {/*this.state.executions.map(e => {
                            const qe = this.state.questionExecutions.find(qe => qe.executionId === e.id);

                            return qe ? (
                                <td key={qe.id}>{qe.position}</td>
                            ) : <td></td>;
                        })*/}
                    </tr>
                    {/*this.state.possibleResponses.map(pr => (
                        <tr key={pr.id}>
                            <th scope="row">{pr.name}</th>
                            {this.state.executions.map(e => {
                                const qes = this.state.questionExecutions.filter(qe => qe.executionId === e.id).map(qe => qe.id);
                                const r = this.state.responses.find(r => r.possibleResponseId === pr.id && qes.includes(r.questionExecutionId));

                                return r ? (
                                    <td key={r.id}>{pr.partOfPercentage ? r.percentage.toFixed(1) : r.count}{pr.partOfPercentage && '%'}</td>
                                ) : <td></td>;
                            })}
                        </tr>
                    ))*/}
                </tbody>
            </Table>
        );
    }

    async populateResultsData() {
        const response = await fetch('api/results?' + new URLSearchParams({ 'question-ids': [48], 'data-group-names': ['Dummy'] }));
        const results = await response.json();

        const grouped = _.groupBy(results, r => r.possibleResponseName);
        Object.keys(grouped).forEach(key => {
            grouped[key] = grouped[key].sort((a, b) => b.executionTime - a.executionTime);
        });

        console.log(grouped);
    }
}
