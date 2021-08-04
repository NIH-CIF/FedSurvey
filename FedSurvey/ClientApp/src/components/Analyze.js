import React, { Component } from 'react';
import { Button, Row, Col } from 'reactstrap';
import { Link } from 'react-router-dom';
import _ from 'lodash';
import { Advanced } from './Advanced';
import { ResultsDataTable } from './ResultsDataTable';

export class Analyze extends Component {
    static displayName = Analyze.name;

    constructor(props) {
        super(props);
        this.state = {
            groupingVariable: null,
            sortingVariable: null,
            filters: {},
            showDifference: false,
            mode: null
        };
    }

    componentDidMount() {
        this.populateDropdownData();
    }

    render() {
        return (
            <div>
                <div style={{ display: 'flex', justifyContent: 'space-between' }}>
                    <Link to='/'>Home</Link>

                    <Button
                        color="link"
                        onClick={this.reset.bind(this)}
                        style={{ padding: 0, border: 0 }}
                    >
                        Reset
                    </Button>
                </div>

                {this.state.mode === null ? (
                    <Row>
                        <Col onClick={e => this.setState(
                            {
                                mode: 'top-pos-org',
                                sortingVariable: 'dataGroupName',
                                groupingVariable: 'questionId',
                                filters: {
                                    'possible-response-names': ['Positive'],
                                    'execution-keys': ['2020'],
                                    'data-group-names': ['OSC TOTAL', 'DIV PRGM COORD, PLNG & STRATEGIC INITIATIVES', 'OFFICE OF THE DIRECTOR (OD)']
                                }
                            }
                        )}>
                            Top Positive Responses
                        </Col>
                        <Col onClick={e => this.setState(
                            {
                                mode: 'top-pos-org',
                                sortingVariable: 'dataGroupName',
                                groupingVariable: 'questionId',
                                filters: {
                                    'possible-response-names': ['Neutral'],
                                    'execution-keys': ['2020'],
                                    'data-group-names': ['OSC TOTAL', 'DIV PRGM COORD, PLNG & STRATEGIC INITIATIVES', 'OFFICE OF THE DIRECTOR (OD)']
                                }
                            }
                        )}>
                            Top Neutral Responses
                        </Col>
                        <Col onClick={e => this.setState(
                            {
                                mode: 'top-pos-org',
                                sortingVariable: 'dataGroupName',
                                groupingVariable: 'questionId',
                                filters: {
                                    'possible-response-names': ['Negative'],
                                    'execution-keys': ['2020'],
                                    'data-group-names': ['OSC TOTAL', 'DIV PRGM COORD, PLNG & STRATEGIC INITIATIVES', 'OFFICE OF THE DIRECTOR (OD)']
                                }
                            }
                        )}>
                            Top Negative Responses
                        </Col>
                        <Col onClick={e => this.setState({ mode: 'advanced' })}>
                            Advanced
                        </Col>
                    </Row>
                ) : (
                    this.state.mode === 'advanced' && (
                        <Advanced
                            groupingVariable={this.state.groupingVariable}
                            sortingVariable={this.state.sortingVariable}
                            filters={this.state.filters}
                            showDifference={this.state.showDifference}
                            nonEmptyFiltersCount={this.getNonEmptyFiltersCount()}
                            updateFilters={this.updateFilters.bind(this)}
                            updateTableVariable={this.updateTableVariable.bind(this)}
                            updateShowDifference={this.updateShowDifference.bind(this)}
                        />
                    )
                )}

                {this.state.groupingVariable && this.state.sortingVariable && this.getNonEmptyFiltersCount() > 1 && (
                    <div>
                        <ResultsDataTable
                            filters={this.state.filters}
                            groupingVariable={this.state.groupingVariable}
                            sortingVariable={this.state.sortingVariable}
                            showDifference={this.state.showDifference}
                            sortable
                            downloadable
                        />
                    </div>
                )}
            </div>
        );
    }

    reset() {
        this.setState({
            sortingVariable: null,
            groupingVariable: null,
            showDifference: false,
            filters: {},
            mode: null
        });
    }

    getNonEmptyFiltersCount() {
        // needs to move
        Object.filter = (obj, predicate) =>
            Object.fromEntries(Object.entries(obj).filter(predicate));

        return Object.keys(Object.filter(this.state.filters, ([key, value]) => value.length > 0)).length;
    }

    updateFilters(dropdownFilterVariable, value) {
        const setValue = Array.isArray(value) ? value.map(v => v.value) : [value.value];

        this.setState(prevState => ({
            filters: { ...prevState.filters, [dropdownFilterVariable.filterKey]: setValue }
        }));
    }

    updateTableVariable(variableName, currentDropdownVariable, dropdownFilterVariable, value) {
        this.setState(prevState => ({
            filters: dropdownFilterVariable !== null ? {
                ...prevState.filters,
                [dropdownFilterVariable.filterKey]: []
            } : prevState.filters,
            [variableName]: value,
        }));
    }

    updateShowDifference(val) {
        this.setState({ showDifference: val });
    }

    async populateDropdownData() {
        // get questions, data groups, executions, possible responses
        // maybe a questions API route for easier processing later
        const response = await Promise.all(
            [
                fetch('api/question-executions'),
                fetch('api/executions'), // later will be an if to be included based on config
                fetch('api/data-groups'),
                fetch('api/possible-responses')
            ]
        );
        const [questionExecutions, executions, dataGroups, possibleResponses] = await Promise.all(
            response.map(r => r.json())
        );

        const groupedQuestionExecutions = Object.entries(_.groupBy(questionExecutions, qe => qe.questionId)).map(([key, value]) => ({
            body: value[0].body,
            questionId: value[0].questionId,
            id: value[0].id
        }));

        this.setState({
            questionExecutions: groupedQuestionExecutions,
            executions,
            dataGroups,
            possibleResponses
        });
    }
}
