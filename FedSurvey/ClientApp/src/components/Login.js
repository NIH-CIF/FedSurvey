import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { Button, FormGroup, Input, Label } from 'reactstrap';
import Cookies from 'universal-cookie';

export class Login extends Component {
    static displayName = Login.name;

    constructor(props) {
        super(props);
        this.state = {
            username: '',
            password: '',
            processing: false,
            success: null
        };
    }

    render() {
        return (
            <div>
                <div>
                    <Link to="/">Home</Link>
                </div>

                <h4>Log In</h4>

                <FormGroup>
                    <Label for="username">Username</Label>
                    <Input type="text" onChange={e => this.setState({ username: e.target.value })} />
                </FormGroup>
                <FormGroup>
                    <Label for="password">Username</Label>
                    <Input type="password" onChange={e => this.setState({ password: e.target.value })} />
                </FormGroup>

                <Button color="primary" onClick={this.submit.bind(this)} disabled={this.state.username.length === 0 || this.state.password.length === 0}>Submit</Button>

                {this.state.processing && (<p>Processing log in...</p>)}
                {this.state.success === true && (
                    <p>
                        Log in success!
                        Press "Home" above to view data.
                    </p>
                )}
                {this.state.success === false && (
                    <p>
                        Log in failed!
                        Please try again.
                    </p>
                )}
            </div>
        );
    }

    submit() {
        this.setState({ processing: true });

        fetch('api/tokens', {
            method: 'post',
            body: JSON.stringify({
                username: this.state.username,
                password: this.state.password
            }),
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(response => {
                this.setState({ success: response.status === 200, processing: false });

                response.json()
                    .then(j => {
                        const today = new Date();

                        const cookies = new Cookies();
                        cookies.set('token', j.body, {
                            expires: new Date(today.getFullYear(), today.getMonth(), today.getDate() + 7)
                        });
                    });
            });
    }
}
