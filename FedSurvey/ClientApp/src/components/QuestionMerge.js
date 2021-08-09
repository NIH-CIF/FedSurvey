import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { Button, ButtonGroup, FormGroup } from 'reactstrap';
import Select from 'react-select';
import _ from 'lodash';

export class QuestionMerge extends Component {
    static displayName = QuestionMerge.name;

    constructor(props) {
        super(props);
        this.state = {
            questionTexts: [],
            mergeCandidates: [],
            toMerge: [],
            loading: true,
            processing: false,
            success: null
        };
    }

    componentDidMount() {
        this.populateQuestionTexts();
    }

    render() {
        return !this.state.loading && (
            <div>
                <div>
                    <Link to="/" style={{ marginRight: 40 }}>Home</Link>

                    <Link to="/data-groups/merge" style={{ marginRight: 40 }}>Merge Organizations</Link>

                    <Link to="/data-groups/create" style={{ marginRight: 40 }}>Create Computed Organizations</Link>

                    <Link to="/admin">Admin</Link>
                </div>

                <h4>Merge questions</h4>

                <span>View the list of question texts within this select and then type the questions you would like to merge into the select.</span>

                <FormGroup>
                    <Select
                        name="mergeSelect"
                        isMulti
                        id="mergeSelect"
                        onChange={val => this.setState(prevState => ({ toMerge: val.map(v => v.value) }))}
                        value={this.state.toMerge.map(mt => ({ label: mt, value: mt }))}
                        options={this.state.questionTexts.map(item => ({ label: item, value: item }))}
                    />
                </FormGroup>

                <ButtonGroup>
                    <Button color="primary" onClick={this.submit.bind(this)} disabled={this.state.toMerge.length < 2}>Merge</Button>

                    <Button onClick={this.reset.bind(this)}>Reset</Button>
                </ButtonGroup>

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

                {this.state.mergeCandidates.length > 0 && (
                    <>
                        <h5>Merge Candidates</h5>

                        <p>
                            These are questions that have only occurred in a single year so far.
                            It is possible that they have a slight text variation that led to this.
                        </p>
                    </>
                )}

                {this.state.mergeCandidates.map(mc => (
                    <p>Question Number {mc.position} in {mc.executionKey}: {mc.body}</p>
                ))}
            </div>
        );
    }

    reset() {
        this.setState({
            questionTexts: [],
            mergeCandidates: [],
            toMerge: [],
            loading: true,
            processing: false,
            success: null
        });
        this.populateQuestionTexts();
    }

    submit() {
        this.setState({ processing: true });

        fetch('api/questions/merge', {
            method: 'post',
            body: JSON.stringify(this.state.toMerge),
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(response => this.setState({ success: response.status === 200, processing: false }));
    }

    async populateQuestionTexts() {
        // would be good to show all strings for each data group listed here
        const response = await Promise.all(
            [
                fetch('api/question-executions'),
                fetch('api/questions/merge-candidates')
            ]
        );
        const [questionExecutions, mergeCandidates] = await Promise.all(
            response.map(r => r.json())
        );
        const duplicatesRemoved = Object.keys(_.groupBy(questionExecutions, qe => qe.body)).sort((a, b) => (a < b) ? -1 : ((b < a) ? 1 : 0));

        this.setState({ questionTexts: duplicatesRemoved, mergeCandidates: mergeCandidates.sort((a, b) => a.position - b.position), loading: false });
    }
}
