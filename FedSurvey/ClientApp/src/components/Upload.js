import React, { Component } from 'react';
import { Button, Modal, ModalHeader, ModalBody, ModalFooter, Form, FormGroup, FormText, Label, Input } from 'reactstrap';
import { Link } from 'react-router-dom';
import api from '../api';

export class Upload extends Component {
    static displayName = Upload.name;

    constructor(props) {
        super(props);
        this.state = {
            isOpen: false,
            format: null,
            fileInputKey: Date.now(),
            file: null,
            key: '',
            dataGroupName: '',
            uploadSuccess: null,
            uploading: false
        };
    }

    render() {
        const toggle = this.toggle.bind(this);

        return (
            <Button outline block color="danger" onClick={toggle} style={this.props.style}>
                Upload
                <Modal isOpen={this.state.isOpen} toggle={toggle}>
                    <ModalHeader toggle={toggle}>Upload</ModalHeader>
                    <Form>
                        <ModalBody>
                            <FormGroup>
                                <Label for="file">File</Label>
                                <Input
                                    type="file"
                                    name="file"
                                    id="file"
                                    onChange={e => this.handleFileChange(e.target)}
                                    key={this.state.fileInputKey}
                                />
                                {this.state.format === 'unknown' && (
                                    <FormText>
                                        This file cannot be uploaded.
                                    </FormText>
                                )}
                            </FormGroup>

                            <FormGroup>
                                <Label for="key">Year</Label>
                                <Input type="text" name="key" id="key" disabled={!this.state.format || this.state.format === 'unknown'} onChange={e => this.setState({ key: e.target.value })} value={this.state.key} />
                            </FormGroup>

                            {this.state.format === 'survey-monkey' && (
                                <FormGroup>
                                    <Label for="key">Organization Name</Label>
                                    <Input
                                        type="text"
                                        name="dataGroupName"
                                        id="dataGroupName"
                                        onChange={e => this.setState({ dataGroupName: e.target.value })}
                                        value={this.state.dataGroupName}
                                    />
                                </FormGroup>
                            )}

                            {this.state.uploadSuccess && (
                                <p>
                                    Upload success!
                                    Click
                                    <span style={{ cursor: 'pointer', color: 'blue', marginLeft: 4, marginRight: 4 }} onClick={this.reset.bind(this)}>here</span>
                                    to upload more.
                                    You may need to
                                    <Link to='/data-groups/merge' style={{ marginLeft: 4 }}>merge data groups</Link>,
                                    <Link to='/data-groups/create' style={{ marginLeft: 4 }}>create a computed data group</Link>,
                                    or <Link to='/questions/merge'>merge questions</Link>.
                                </p>
                            )}
                        </ModalBody>
                        <ModalFooter>
                            <Button color="danger" disabled={this.state.uploading || !this.state.file || !this.state.key || (this.state.format === 'survey-monkey' && !this.state.dataGroupName)}  onClick={this.submitUpload.bind(this)}>Upload</Button>
                            <Button color="secondary" disabled={this.state.uploading} onClick={toggle}>Cancel</Button>
                        </ModalFooter>
                    </Form>
                    </Modal>
                </Button>
        );
    }

    reset() {
        this.setState({
            format: null,
            fileInputKey: Date.now(),
            file: '',
            key: '',
            dataGroupName: '',
            uploadSuccess: null,
            uploading: false
        });
    }

    toggle() {
        this.setState(prevState => ({ isOpen: !prevState.isOpen }));
    }

    async handleFileChange(target) {
        const data = new FormData();
        data.append('file', target.files[0]);

        const res = await api.fetch('api/upload/format', {
            method: 'POST',
            body: data
        });
        const result = await res.json();

        this.setState({ file: result.format !== 'unknown' ? target.files[0] : null, format: result.format });
    }

    async submitUpload() {
        this.setState({ uploading: true });

        const data = new FormData();
        data.append('file', this.state.file);
        data.append('key', this.state.key);

        if (this.state.dataGroupName)
            data.append('dataGroupName', this.state.dataGroupName);

        const res = await api.fetch('api/upload', {
            method: 'POST',
            body: data
        });

        if (res.ok)
            this.setState({ uploadSuccess: true });
    }
}
