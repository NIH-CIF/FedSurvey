import React, { Component } from 'react';
import { Button, Input, Label } from 'reactstrap';
import { QuestionList } from './QuestionList';
import { Upload } from './Upload';
import { Link } from 'react-router-dom';

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
                <div style={{ display: 'flex', alignItems: 'center' }}>
                    <div style={{ flex: 0, alignSelf: 'flex-end' }}>
                        <Upload />
                    </div>

                    <div style={{ flex: 1 }}>
                        <Label for="executionSelect">Year</Label>
                        <Input type="select" name="executionSelect" id="executionSelect" onChange={e => this.setState({ currentExecutionId: e.target.value })} value={this.state.currentExecutionId}>
                            {this.state.executions.map(e => (
                                <option value={e.id} key={e.id}>{e.key}</option>
                            ))}
                        </Input>
                    </div>

                    <div style={{ flex: 0, alignSelf: 'flex-end' }}>
                        <Link to='/analyze'>
                            <Button outline block color="secondary">
                                Analyze
                            </Button>
                        </Link>
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
        this.setState({ executions: data, currentExecutionId: data[0].id, loading: false });
    }
}
