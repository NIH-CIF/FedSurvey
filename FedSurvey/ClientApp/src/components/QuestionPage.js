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
            questionExecutions: [], responses: [], possibleResponses: [], dataGroups: [], executions: [], latestQuestionExecution: {}, currentDataGroupId: 0, loading: true };
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
                        <Input type="select" name="dataGroupSelect" id="dataGroupSelect" onChange={e => { this.setState({ currentDataGroupId: e.target.value }); this.populateQuestionData() }} value={this.state.currentDataGroupId} style={{ flexShrink: 2100 }}>
                            {this.state.dataGroups.map(dg => (
                                <option value={dg.id} key={dg.id}>{dg.name}</option>
                            ))}
                        </Input>
                    </div>
                </div>

                <h2>{this.state.latestQuestionExecution.body}</h2>

                <ResultsDataTable />

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

        // Later, this needs to be in state.
        const currentDataGroupId = this.state.currentDataGroupId === 0 ? dataGroups[0].id : this.state.currentDataGroupId;

        response = await fetch('api/responses?' + new URLSearchParams(questionExecutions.map(qe => ['question-execution-ids', qe.id]).concat([['data-group-ids', currentDataGroupId]])));
        const responses = await response.json();

        // uniqueness not working right
        response = await fetch('api/possible-responses?' + new URLSearchParams([...new Set(responses.map(r => ['ids', r.possibleResponseId]))]));
        const possibleResponses = await response.json();

        // prepare question text changes
        const questionTexts = [...new Set(questionExecutions.map(qe => qe.body))];
        const questionYears = Object.assign({}, ...questionTexts.map(qt => ({ [Math.min(...questionExecutions.filter(qe => qe.body === qt).map(qe => qe.executionId).map(eid => executions.find(e => e.id === eid).key))]: qt })));

        const questionChanges = Object.keys(questionYears).length > 1 ? questionYears : {};

        this.setState({ dataGroups: dataGroups, questionExecutions: questionExecutions, responses: responses, executions: executions, possibleResponses: possibleResponses, latestQuestionExecution: latestQuestionExecution, questionChanges: questionChanges, currentDataGroupId: currentDataGroupId, loading: false });
    }
}
