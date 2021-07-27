import React, { Component } from 'react';
import { Table } from 'reactstrap';
import { Input, Label } from 'reactstrap';
import { Link } from 'react-router-dom';
import { ResultsDataTable } from './ResultsDataTable';

export class QuestionPage extends Component {
    static displayName = QuestionPage.name;

    constructor(props) {
        super(props);
        this.state = {
            questionExecutions: [], responses: [], possibleResponses: [], dataGroups: [], executions: [], latestQuestionExecution: {}, currentDataGroupName: 0, loading: true };
    }

    componentDidMount() {
        this.populateQuestionData();
    }

    render() {
        return !this.state.loading && (
            <div>
                <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                    <Link to='/'>Home</Link>

                    <div style={{ display: 'flex', alignItems: 'center' }}>
                        <Label for="dataGroupSelect" style={{ marginRight: '0.5rem', marginBottom: 0 }}>Organization</Label>
                        <Input type="select" name="dataGroupSelect" id="dataGroupSelect" onChange={e => { this.setState({ currentDataGroupName: e.target.value }); this.populateQuestionData() }} value={this.state.currentDataGroupName} style={{ flexShrink: 2100 }}>
                            {this.state.dataGroups.map(dg => (
                                <option value={dg.name} key={dg.id}>{dg.name}</option>
                            ))}
                        </Input>
                    </div>
                </div>

                <h2>{this.state.latestQuestionExecution.body}</h2>

                <ResultsDataTable
                    filters={{
                        'question-ids': [this.props.match.params.questionId],
                        'data-group-names': [this.state.currentDataGroupName]
                    }}
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
        let response = await Promise.all(
            [
                fetch('api/question-executions?' + new URLSearchParams({ 'question-ids': this.props.match.params.questionId })),
                fetch('api/data-groups'),
                // to-do filter executions by ids
                fetch('api/executions')
            ]    
        );
        const dataGroups = await response[1].json();
        const questionExecutions = await response[0].json();
        const executions = await response[2].json();
        const latestExecutionId = Math.max(...questionExecutions.map(qe => qe.executionId));
        const latestQuestionExecution = questionExecutions.find(qe => qe.executionId === latestExecutionId);

        const currentDataGroupName = this.state.currentDataGroupName !== null ? dataGroups[0].name : this.state.currentDataGroupName;

        // prepare question text changes
        const questionTexts = [...new Set(questionExecutions.map(qe => qe.body))];
        const questionYears = Object.assign({}, ...questionTexts.map(qt => ({ [Math.min(...questionExecutions.filter(qe => qe.body === qt).map(qe => qe.executionId).map(eid => executions.find(e => e.id === eid).key))]: qt })));

        const questionChanges = Object.keys(questionYears).length > 1 ? questionYears : {};

        this.setState({ dataGroups: dataGroups, questionExecutions: questionExecutions, executions: executions, latestQuestionExecution: latestQuestionExecution, questionChanges: questionChanges, currentDataGroupName: currentDataGroupName, loading: false });
    }
}
