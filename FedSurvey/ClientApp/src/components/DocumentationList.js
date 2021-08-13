import React, { Component } from 'react';
import { Documentation } from './Documentation';
import api from '../api';
import { Button } from 'reactstrap';

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
                <h2>
                    {this.state.selectedFile !== null && (
                        <Button outline onClick={e => this.setState({ selectedFile: null })}>&larr;</Button>
                    )}

                    Documentation
                </h2>

                {this.state.selectedFile !== null ? (
                    <Documentation file={this.state.selectedFile} />
                ) : (
                    <ul>
                        {this.state.markdownFiles.map(mf => (
                            <li key={mf}>
                                <span style={{ color: 'blue', cursor: 'pointer' }} onClick={e => this.setState({ selectedFile: mf })}>
                                    {mf.replace('.md', '').split('-').map(s => s.charAt(0).toUpperCase() + s.substring(1)).join(' ')}
                                </span>
                            </li>
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
