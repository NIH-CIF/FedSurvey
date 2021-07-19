import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { FormGroup, Input, Label } from 'reactstrap';

export class DataGroupMerge extends Component {
    static displayName = DataGroupMerge.name;

    constructor(props) {
        super(props);
        this.state = {
            dataGroups: [], checked: [], mergeTo: null, step: 0, loading: true
        };
    }

    componentDidMount() {
        this.populateDataGroups();
    }

    render() {
        return !this.state.loading && (
            <div>
                <div>
                    <Link to='/'>Home</Link>
                </div>

                <h4>Merge data groups</h4>

                <span>{this.getTextForStep()}</span>

                <FormGroup>
                    {this.state.dataGroups.filter(dg => !this.state.checked.map(dg => dg.id).includes(dg.id)).map(dg => (
                        <FormGroup check key={dg.id}>
                            <Label check>
                                <Input type="checkbox" onChange={e => this.handleCheck(dg)} />{' '}
                                {dg.name}
                            </Label>
                        </FormGroup>
                    ))}
                </FormGroup>

                <p>
                    Merging {this.state.checked.map(checked => checked.name).join(', ') || '? '}
                    into a group of the name {this.state.mergeTo?.name || '?'}
                </p>
            </div>
        );
    }

    getTextForStep() {
        if (this.state.step === 0) {
            return 'First, check the box next to the data group that you want to maintain its name through this merge.'
        } else if (this.state.step === 1) {
            return 'Now, select the groups that you would like to merge with the previous group.';
        } else {
            return 'Not sure what to do here';
        }
    }

    handleCheck(dataGroup) {
        this.setState({ checked: this.state.checked.concat(dataGroup), mergeTo: this.state.mergeTo || dataGroup, step: 1 });
    }

    async populateDataGroups() {
        // would be good to show all strings for each data group listed here
        const dataGroups = await (await fetch('api/data-groups')).json();

        this.setState({ dataGroups: dataGroups, loading: false });
    }
}
