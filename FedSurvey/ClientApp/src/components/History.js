import React, { Component } from 'react';
import { Button, Input, Label } from 'reactstrap';
import { QuestionList } from './QuestionList';
import { Link } from 'react-router-dom';

export class History extends Component {
    static displayName = History.name;

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
                <div style={{ display: 'flex', alignItems: 'center' }}>
                    <div style={{ flex: 0, alignSelf: 'flex-end' }}>
                        <Link to='/'>
                            <Button outline block color="secondary">
                                Home
                            </Button>
                        </Link>
                    </div>

                    <div style={{ flex: 1 }}>
                        <Label for="executionSelect">Year</Label>
                        <Input type="select" name="executionSelect" id="executionSelect" onChange={e => this.setState({ currentExecutionId: e.target.value })} value={this.state.currentExecutionId}>
                            {this.state.executions.map(e => (
                                <option value={e.id} key={e.id}>{e.key}</option>
                            ))}
                        </Input>
                    </div>
                </div>

                <hr />

                <QuestionList executionId={this.state.currentExecutionId} />
            </div>
        );
    }

    async populateExecutionData() {
        const response = await fetch('api/executions');
        const data = await response.json();
        this.setState({ executions: data, currentExecutionId: data[data.length - 1].id, loading: false });
    }
}
