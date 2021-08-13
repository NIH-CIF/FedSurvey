import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { Button, ButtonGroup } from 'reactstrap';
import { Upload } from './Upload';
import { Documentation } from './Documentation';

export class Admin extends Component {
    static displayName = Admin.name;

    render() {
        return (
            <div>
                <div>
                    <Link to='/'>Home</Link>
                </div>

                <h4>Admin Tasks</h4>

                <ButtonGroup style={{ width: '100%', display: 'flex' }}>
                    <Button
                        outline
                        color="secondary"
                        tag={Link}
                        to="/data-groups/merge"
                        style={{ flex: 1 }}
                    >
                        Merge Organizations
                    </Button>

                    <Button
                        outline
                        color="secondary"
                        tag={Link}
                        to="/data-groups/create"
                        style={{ flex: 1 }}
                    >
                        Create Computed Data Group
                    </Button>

                    <Button
                        outline
                        color="secondary"
                        tag={Link}
                        to="/questions/merge"
                        style={{ flex: 1 }}
                    >
                        Merge Questions
                    </Button>

                    <Upload
                        style={{ flex: 1 }}
                    />
                </ButtonGroup>

                <Documentation file="organization-merge.md" />
            </div>
        );
    }
}
