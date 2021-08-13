import React, { Component } from 'react';
import ReactMarkdown from 'react-markdown';
import api from '../api';

export class DocumentationList extends Component {
    static displayName = DocumentationList.name;

    constructor(props) {
        super(props);
        this.state = {
            markdownFiles: [],
            selectedFile: null
        };
    }

    componentDidMount() {
        this.populateMarkdownFiles();
    }

    render() {
        return this.state.markdownFiles.length > 0 && (
            <div>
                <h2>Documentation</h2>

                {this.state.selectedFile !== null ? (
                    <Documentation file={this.state.selectedFile} />
                ) : (
                    <ul>
                        {this.state.markdownFiles.map(mf => (
                            <li key={mf}>{mf.replace('.md', '').split('-').map(s => s.charAt(0).toUpperCase() + s.substring(1)).join(' ')}</li>
                        ))}
                    </ul>
                )}
            </div>
        );
    }

    async populateMarkdownFiles() {
        api.fetch('api/documentation')
            .then(async (resp) => this.setState({ markdownFiles: await resp.json() }));
    }
}
