import React, { Component } from 'react';
import { Input, Label } from 'reactstrap';

export class Home extends Component {
    static displayName = Home.name;

    constructor(props) {
        super(props);
        this.state = { executions: [] };
    }

    componentDidMount() {
        this.populateExecutionData();
    }

    f(thing) { console.log(thing) };

    render() {
        return (
            <div>
                <Label for="executionSelect">Year</Label>
                <Input type="select" name="executionSelect" id="executionSelect" onChange={e => this.f(e.target.value)}>
                    {this.state.executions.map(e => (
                        <option value={e.id} key={e.id}>{e.key}</option>
                    ))}
                </Input>
            </div>
        );
    }

    async populateExecutionData() {
        const response = await fetch('api/executions');
        const data = await response.json();
        this.setState({ executions: data });
    }
}
