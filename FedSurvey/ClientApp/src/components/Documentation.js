import React, { Component } from 'react';
import ReactMarkdown from 'react-markdown';
import api from '../api';

export class Documentation extends Component {
    static displayName = Documentation.name;

    constructor(props) {
        super(props);
        this.state = { markdown: null };
    }

    componentDidMount() {
        this.populateMarkdown();
    }

    render() {
        return this.state.markdown && (
            <ReactMarkdown children={this.state.markdown} />
        );
    }

    async populateMarkdown() {
        api.fetch(`docs/${this.props.file}`)
            .then(async (resp) => this.setState({ markdown: await resp.text() }));
    }
}
