﻿import React, { Component } from 'react';
import { Input, Label } from 'reactstrap';
import { Link } from 'react-router-dom';
import { ResultsDataTable } from './ResultsDataTable';
import api from '../api';

export class QuestionPage extends Component {
    static displayName = QuestionPage.name;

    constructor(props) {
        super(props);
        this.state = {
            dataGroups: [],
            latestQuestionBody: null,
            questionChanges: {},
            currentDataGroupName: null,
            loading: true
        };
    }

    componentDidMount() {
        this.populateQuestionData();
    }

    render() {
        return !this.state.loading && (
            <div>
                <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                    <Link to='/history' style={{ flex: 0.15 }}>History</Link>
                    <Link to='/' style={{ flex: 0.15 }}>Home</Link>

                    <div style={{ display: 'flex', alignItems: 'center', flex: 0.7 }}>
                        <Label for="dataGroupSelect" style={{ marginRight: '0.5rem', marginBottom: 0 }}>Organization</Label>
                        <Input type="select" name="dataGroupSelect" id="dataGroupSelect" onChange={e => { this.setState({ currentDataGroupName: e.target.value }) }} value={this.state.currentDataGroupName}>
                            {this.state.dataGroups.map(dg => (
                                <option value={dg.name} key={dg.id}>{dg.name}</option>
                            ))}
                        </Input>
                    </div>
                </div>

                <h2>{this.state.latestQuestionBody}</h2>

                <ResultsDataTable
                    filters={{
                        'question-ids': [this.props.match.params.questionId],
                        'data-group-names': [this.state.currentDataGroupName]
                    }}
                    groupingVariable="possibleResponseName"
                    sortingVariable="executionTime"
                />

                {Object.keys(this.state.questionChanges).length > 0 && (
                    <div>
                        <h4>
                            Question Text Changes:
                        </h4>

                        <ul>
                            {Object.keys(this.state.questionChanges).map(year => (
                                <li key={year}>{year}: {this.state.questionChanges[year]}</li>
                            ))}
                        </ul>
                    </div>
                )}
            </div>
        );
    }

    async populateQuestionData() {
        const response = await Promise.all(
            [
                api.fetch('api/question-executions?' + new URLSearchParams({ 'question-ids': this.props.match.params.questionId })),
                api.fetch('api/data-groups'),
                api.fetch('api/executions')
            ]    
        );
        const [questionExecutions, dataGroups, executions] = await Promise.all(
            response.map(r => r.json())
        );

        // prepare latest question text for header
        // filtering probably better to be done in API
        const latestExecutionId = executions.filter(e => questionExecutions.map(qe => qe.executionId).includes(e.id)).reduce((prev, current) => (
            prev.executionTime > current.executionTime ? prev : current
        )).id;
        const latestQuestionExecution = questionExecutions.find(qe => qe.executionId === latestExecutionId);
        const latestQuestionBody = latestQuestionExecution.body;

        const currentDataGroupName = this.state.currentDataGroupName !== null ? this.state.currentDataGroupName : dataGroups[0].name;

        // prepare question text changes
        const questionTexts = [...new Set(questionExecutions.map(qe => qe.body))];
        const questionYears = Object.assign({}, ...questionTexts.map(qt => ({ [Math.min(...questionExecutions.filter(qe => qe.body === qt).map(qe => qe.executionId).map(eid => executions.find(e => e.id === eid).key))]: qt })));

        const questionChanges = Object.keys(questionYears).length > 1 ? questionYears : {};

        this.setState({
            dataGroups: dataGroups,
            latestQuestionBody: latestQuestionBody,
            questionChanges: questionChanges,
            currentDataGroupName: currentDataGroupName,
            loading: false
        });
    }
}
