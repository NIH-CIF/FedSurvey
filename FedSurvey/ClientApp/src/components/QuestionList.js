import React, { Component } from 'react';
import { ListGroup, ListGroupItem } from 'reactstrap';
import { Link } from 'react-router-dom';
import api from '../api';

export class QuestionList extends Component {
    static displayName = QuestionList.name;

    constructor(props) {
        super(props);
        this.state = { questionExecutions: [] };
    }

    componentDidMount() {
        this.populateQuestionExecutionData();
    }

    componentDidUpdate(prevProps) {
        this.populateQuestionExecutionData(prevProps);
    }

    render() {
        return (
            <div>
                <ListGroup>
                    {this.state.questionExecutions.map(qe => (
                        <Link to={`/questions/${qe.questionId}`} key={ qe.id }>
                            <ListGroup horizontal style={{paddingBottom: '0.8rem'}}>
                                <ListGroupItem>
                                    {qe.position}
                                </ListGroupItem>
                                <ListGroupItem>
                                    {qe.body}
                                </ListGroupItem>
                            </ListGroup>
                        </Link>
                    ))}
                </ListGroup>
            </div>
        );
    }

    async populateQuestionExecutionData(prevProps) {
        if (!this.props.executionId || (prevProps && prevProps.executionId === this.props.executionId))
            return;

        const response = await api.fetch('api/question-executions?' + new URLSearchParams({ 'execution-ids': [this.props.executionId] }));
        const data = await response.json();
        this.setState({ questionExecutions: data });
    }
}
