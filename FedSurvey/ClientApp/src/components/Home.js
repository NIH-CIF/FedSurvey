import React, { Component } from 'react';
import { Input, Label } from 'reactstrap';
import { QuestionList } from './QuestionList';

export class Home extends Component {
    static displayName = Home.name;

    constructor(props) {
        super(props);
        this.state = { executions: [], currentExecutionId: 0, loading: true };
    }

    componentDidMount() {
        this.populateExecutionData();
    }

    render() {
        return !this.state.loading && (
            <div>
                <Label for="executionSelect">Year</Label>
                <Input type="select" name="executionSelect" id="executionSelect" onChange={e => this.setState({ currentExecutionId: e.target.value })} value={this.state.currentExecutionId}>
                    {this.state.executions.map(e => (
                        <option value={e.id} key={e.id}>{e.key}</option>
                    ))}
                </Input>

                <hr />

                <QuestionList executionId={this.state.currentExecutionId} />
            </div>
        );
    }

    async populateExecutionData() {
        const response = await fetch('api/executions');
        const data = await response.json();
        this.setState({ executions: data, currentExecutionId: data[0].id, loading: false });
    }
}
