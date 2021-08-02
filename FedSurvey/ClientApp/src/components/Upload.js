import React, { Component } from 'react';
import { Button, Modal, ModalHeader, ModalBody, ModalFooter, Form, FormGroup, FormText, Label, Input } from 'reactstrap';

export class Upload extends Component {
    static displayName = Upload.name;

    constructor(props) {
        super(props);
        this.state = {
            isOpen: false,
            format: null,
            file: null,
            key: null,
            dataGroupName: null,
            uploadSuccess: null,
            uploading: false
        };
    }

    render() {
        const toggle = this.toggle.bind(this);

        return (
            <div>
                <Button outline block color="danger" onClick={toggle}>Upload</Button>
                <Modal isOpen={this.state.isOpen} toggle={toggle}>
                    <ModalHeader toggle={toggle}>Upload</ModalHeader>
                    <Form>
                        <ModalBody>
                            <FormGroup>
                                <Label for="file">File</Label>
                                <Input type="file" name="file" id="file" onChange={e => this.handleFileChange(e.target)} />
                                {this.state.format === 'unknown' && (
                                    <FormText>
                                        This file cannot be uploaded.
                                    </FormText>
                                )}
                            </FormGroup>

                            <FormGroup>
                                <Label for="key">Year</Label>
                                <Input type="text" name="key" id="key" disabled={!this.state.format || this.state.format === 'unknown'} onChange={e => this.setState({ key: e.target.value })} />
                            </FormGroup>

                            {this.state.format === 'survey-monkey' && (
                                <FormGroup>
                                    <Label for="key">Organization Name</Label>
                                    <Input type="text" name="dataGroupName" id="dataGroupName" onChange={e => this.setState({ dataGroupName: e.target.value })} />
                                </FormGroup>
                            )}

                            {this.state.uploadSuccess && (
                                <p>
                                    Upload success!
                                    Close and reopen the modal to upload more.
                                </p>
                            )}
                        </ModalBody>
                        <ModalFooter>
                            <Button color="danger" disabled={this.state.uploading || !this.state.file || !this.state.key || (this.state.format === 'survey-monkey' && !this.state.dataGroupName)}  onClick={this.submitUpload.bind(this)}>Upload</Button>
                            <Button color="secondary" onClick={toggle}>Cancel</Button>
                        </ModalFooter>
                    </Form>
                </Modal>
            </div>
        );
    }

    toggle() {
        // State not seem to be held through, so possibly useless code.
        if (this.state.uploadSuccess && this.state.isOpen) {
            this.setState({
                isOpen: false,
                format: null,
                file: null,
                key: null,
                dataGroupName: null,
                uploadSuccess: null,
                uploading: false
            });
        } else {
            this.setState(prevState => ({ isOpen: !prevState.isOpen }));
        }
    }

    async handleFileChange(target) {
        const data = new FormData();
        data.append('file', target.files[0]);

        const res = await fetch('api/upload/format', {
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

        const res = await fetch('api/upload', {
            method: 'POST',
            body: data
        });

        if (res.ok)
            this.setState({ uploadSuccess: true });
    }
}
