import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { Button, ButtonGroup, FormGroup, Input, Label } from 'reactstrap';

// This is exclusive to computed groups for now, but it could become more generic.
// This was copied from DataGroupMerge, so maybe similar code could be put into its own file.
export class DataGroupCreate extends Component {
    static displayName = DataGroupCreate.name;

    constructor(props) {
        super(props);
        this.state = {
            dataGroups: [],
            checked: [],
            newGroupName: null,
            loading: true,
            processing: false,
            success: null
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

                    <Link to="/data-groups/merge" style={{ marginRight: 40 }}>Merge Organizations</Link>

                    <Link to="/questions/merge" style={{ marginRight: 40 }}>Merge Questions</Link>

                    <Link to="/admin">Admin</Link>
                </div>

                <h4>Create computed organization</h4>

                <span>Name the new organization</span>

                <Input type="text" onChange={e => this.setState({ newGroupName: e.target.value })} />

                <span>Check the organizations that will sum to the new organization</span>

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
                    Defining {this.state.newGroupName || '?'} as
                    {' ' + (this.state.checked.length > 0 ? this.state.checked.map(checked => checked.name).join(' + ') : '?')}
                </p>

                <ButtonGroup>
                    <Button color="primary" onClick={this.submit.bind(this)} disabled={this.state.checked.length < 2}>Create</Button>
                    <Button onClick={this.reset.bind(this)}>Reset</Button>
                </ButtonGroup>

                {this.state.processing && (<p>Processing creation...</p>)}
                {this.state.success === true && (
                    <p>
                        Creation success!
                        Press "Home" above to view data with this new group or click
                        <span style={{ cursor: 'pointer', color: 'blue', marginLeft: 4, marginRight: 4 }} onClick={this.reset.bind(this)}>here</span>
                        to create a new group.
                    </p>
                )}
                {this.state.success === false && (
                    <p>
                        Creation failed!
                        Please contact the development team.
                    </p>
                )}
            </div>
        );
    }

    reset() {
        this.setState({
            dataGroups: [],
            checked: [],
            newGroupName: null,
            loading: true,
            processing: false,
            success: null
        });
        this.populateDataGroups();
    }

    handleCheck(dataGroup) {
        this.setState({ checked: this.state.checked.concat(dataGroup) });
    }

    submit() {
        // Prepare the ids for the POST in the correct order.
        this.setState({ processing: true });

        fetch('api/data-groups/create', {
            method: 'post',
            body: JSON.stringify({
                name: this.state.newGroupName,
                linkIds: this.state.checked.map(dg => dg.id)
            }),
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
