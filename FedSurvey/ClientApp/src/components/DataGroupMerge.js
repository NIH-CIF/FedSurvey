import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { Button, FormGroup, Input, Label } from 'reactstrap';

export class DataGroupMerge extends Component {
    static displayName = DataGroupMerge.name;

    constructor(props) {
        super(props);
        this.state = {
            dataGroups: [], checked: [], mergeTo: null, step: 0, loading: true, processing: false, success: null
        };
    }

    componentDidMount() {
        this.populateDataGroups();
    }

    render() {
        return !this.state.loading && (
            <div>
                <div>
                    <Link to="/" style={{ marginRight: 40 }}>Home</Link>

                    <Link to="/data-groups/create" style={{ marginRight: 40 }}>Create Computed Organization</Link>

                    <Link to="/questions/merge" style={{ marginRight: 40 }}>Merge Questions</Link>

                    <Link to="/admin">Admin</Link>
                </div>

                <h4>Merge organizations</h4>

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
                    Merging {(this.state.checked.map(checked => checked.name).join(', ') + ' ') || '? '}
                    into an organization of the name {this.state.mergeTo?.name || '?'}
                </p>

                <Button onClick={this.submit.bind(this)} disabled={this.state.checked.length < 2}>Merge</Button>

                {this.state.processing && (<p>Processing merge...</p>)}
                {this.state.success === true && (
                    <p>
                        Merge success!
                        Press "Home" above to view modified data or
                        click <span style={{ cursor: 'pointer', color: 'blue' }} onClick={this.reset.bind(this)}>here</span> to perform another merge.
                    </p>
                )}
                {this.state.success === false && (
                    <p>
                        Merge failed!
                        Please contact the development team.
                    </p>
                )}
            </div>
        );
    }

    reset() {
        this.setState({
            dataGroups: [], checked: [], mergeTo: null, step: 0, loading: true, processing: false, success: null
        });
        this.populateDataGroups();
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

    submit() {
        // Prepare the ids for the POST in the correct order.
        const ids = this.state.checked.sort((a, b) => (b.id === this.state.mergeTo.id ? 1 : 0) - (a.id === this.state.mergeTo.id ? 1 : 0)).map(dg => dg.id);
        this.setState({ processing: true });

        fetch('api/data-groups/merge', {
            method: 'post',
            body: JSON.stringify(ids),
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(response => this.setState({ success: response.status === 200, processing: false }));
    }

    async populateDataGroups() {
        // would be good to show all strings for each data group listed here
        const dataGroups = await (await fetch('api/data-groups?computed=false')).json();

        this.setState({ dataGroups: dataGroups, loading: false });
    }
}
