﻿import React, { Component } from 'react';
import { Button, ButtonGroup, Row, Col } from 'reactstrap';
import { Advanced } from './Advanced';
import { ResultsDataTable } from './ResultsDataTable';
import { Upload } from './Upload';
import { Redirect } from 'react-router-dom';

export class Analyze extends Component {
    static displayName = Analyze.name;

    constructor(props) {
        super(props);
        this.state = {
            groupingVariable: null,
            sortingVariable: null,
            filters: {},
            showDifference: false,
            sort: {},
            mode: null,
            loading: true,
            latestExecutionNames: null,
            leftButtons: [],
            rightButtons: []
        };
    }

    componentDidMount() {
        this.populateExecutionData();
    }

    linkCol(text, stateChange) {
        return (
            <Col
                onClick={e => this.setState(stateChange)}
                style={{ display: 'flex', alignItems: 'center', justifyContent: 'center', border: '1px solid rgba(0, 0, 0, 0.1)', padding: 0, cursor: 'pointer' }}
                className="linkCol"
            >
                <span style={{ textAlign: 'center' }}>
                    {text}
                </span>
            </Col>
        );
    }

    // Maybe the hardcoded strings should just be in a file for changes to be made.
    render() {
        return !this.state.loading && (
            <div style={{ height: '100%' }}>
                <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                    <ButtonGroup>
                        <Button
                            outline
                            color="secondary"
                            onClick={this.home.bind(this)}
                        >
                            Home
                        </Button>

                        {this.state.leftButtons.map(lb => (
                            <Button
                                key={lb.text}
                                outline
                                color="secondary"
                                onClick={lb.onClick}
                            >
                                {lb.text}
                            </Button>
                        ))}
                    </ButtonGroup>

                    <span>
                        Survey Data Aggregator
                    </span>

                    <ButtonGroup>
                        <Upload />

                        {this.state.rightButtons.map(rb => (
                            <Button
                                key={rb.text}
                                outline
                                color="primary"
                                onClick={rb.onClick}
                            >
                                {rb.text}
                            </Button>
                        ))}
                    </ButtonGroup>
                </div>

                <hr style={{ margin: 0 }} />

                {this.state.mode === null && (
                    <Row xs="3" style={{ height: 'calc(100% - 40px)', width: '100%', margin: 0 }}>
                        {this.linkCol('Top Positive Responses', {
                            mode: 'no-component',
                            sortingVariable: 'dataGroupName',
                            groupingVariable: 'questionId',
                            filters: {
                                'possible-response-names': ['Positive'],
                                'execution-keys': [this.state.latestExecutionNames[0]],
                                'data-group-names': ['OSC TOTAL', 'DIV PRGM COORD, PLNG & STRATEGIC INITIATIVES', 'OFFICE OF THE DIRECTOR (OD)'],
                                'question-group-ids': [1]
                            },
                            sort: {
                                header: 'OSC TOTAL',
                                direction: 'desc'
                            }
                        })}
                        {this.linkCol('Top Neutral Responses', {
                            mode: 'no-component',
                            sortingVariable: 'dataGroupName',
                            groupingVariable: 'questionId',
                            filters: {
                                'possible-response-names': ['Neutral'],
                                'execution-keys': [this.state.latestExecutionNames[0]],
                                'data-group-names': ['OSC TOTAL', 'DIV PRGM COORD, PLNG & STRATEGIC INITIATIVES', 'OFFICE OF THE DIRECTOR (OD)'],
                                'question-group-ids': [1]
                            },
                            sort: {
                                header: 'OSC TOTAL',
                                direction: 'desc'
                            }
                        })}
                        {this.linkCol('Top Negative Responses', {
                            mode: 'no-component',
                            sortingVariable: 'dataGroupName',
                            groupingVariable: 'questionId',
                            filters: {
                                'possible-response-names': ['Negative'],
                                'execution-keys': [this.state.latestExecutionNames[0]],
                                'data-group-names': ['OSC TOTAL', 'DIV PRGM COORD, PLNG & STRATEGIC INITIATIVES', 'OFFICE OF THE DIRECTOR (OD)'],
                                'question-group-ids': [1]
                            },
                            sort: {
                                header: 'OSC TOTAL',
                                direction: 'desc'
                            }
                        })}
                        {this.linkCol('Top Positive Responses Compared to 2016 on', {
                            mode: 'no-component',
                            sortingVariable: 'executionName',
                            groupingVariable: 'questionId',
                            filters: {
                                'possible-response-names': ['Positive'],
                                'data-group-names': ['OSC TOTAL'],
                                'question-group-ids': [1]
                            },
                            showDifference: true,
                            sort: {
                                header: this.state.latestExecutionNames[0],
                                direction: 'desc'
                            }
                        })}
                        {this.linkCol('Top Negative Responses Compared to 2016 on', {
                            mode: 'no-component',
                            sortingVariable: 'executionName',
                            groupingVariable: 'questionId',
                            filters: {
                                'possible-response-names': ['Negative'],
                                'data-group-names': ['OSC TOTAL'],
                                'question-group-ids': [1]
                            },
                            showDifference: true,
                            sort: {
                                header: this.state.latestExecutionNames[0],
                                direction: 'desc'
                            }
                        })}
                        {this.linkCol('Top Positive Response Increases', {
                            mode: 'no-component',
                            sortingVariable: 'executionName',
                            groupingVariable: 'questionId',
                            filters: {
                                'possible-response-names': ['Positive'],
                                'data-group-names': ['OSC TOTAL'],
                                'execution-keys': this.state.latestExecutionNames.slice(0, 2),
                                'question-group-ids': [1]
                            },
                            showDifference: true,
                            sort: {
                                // show two orgs + delta = last index
                                index: 2,
                                direction: 'desc'
                            }
                        })}
                        {this.linkCol('Top Positive Response Decreases', {
                            mode: 'no-component',
                            sortingVariable: 'executionName',
                            groupingVariable: 'questionId',
                            filters: {
                                'possible-response-names': ['Positive'],
                                'data-group-names': ['OSC TOTAL'],
                                'execution-keys': this.state.latestExecutionNames.slice(0, 2),
                                'question-group-ids': [1]
                            },
                            showDifference: true,
                            sort: {
                                // show two orgs + delta = last index
                                index: 2,
                                direction: 'asc'
                            }
                        })}
                        {this.linkCol('Top Positive Response Strengths Relative to DPCPSI', {
                            mode: 'no-component',
                            sortingVariable: 'dataGroupName',
                            groupingVariable: 'questionId',
                            filters: {
                                'possible-response-names': ['Positive'],
                                'data-group-names': ['OSC TOTAL', 'DIV PRGM COORD, PLNG & STRATEGIC INITIATIVES'],
                                'execution-keys': [this.state.latestExecutionNames[0]],
                                'question-group-ids': [1]
                            },
                            showDifference: true,
                            sort: {
                                // show two orgs + delta = last index
                                index: 2,
                                direction: 'desc'
                            }
                        })}
                        {this.linkCol('Top Positive Response Weaknesses Relative to DPCPSI', {
                            mode: 'no-component',
                            sortingVariable: 'dataGroupName',
                            groupingVariable: 'questionId',
                            filters: {
                                'possible-response-names': ['Positive'],
                                'data-group-names': ['OSC TOTAL', 'DIV PRGM COORD, PLNG & STRATEGIC INITIATIVES'],
                                'execution-keys': [this.state.latestExecutionNames[0]],
                                'question-group-ids': [1]
                            },
                            showDifference: true,
                            sort: {
                                // show two orgs + delta = last index
                                index: 2,
                                direction: 'asc'
                            }
                        })}
                        {this.linkCol('History', {mode: 'history'})}
                        {this.linkCol('Advanced', {mode: 'advanced'})}
                    </Row>
                )}
                {this.state.mode === 'advanced' && (
                    <Advanced
                        groupingVariable={this.state.groupingVariable}
                        sortingVariable={this.state.sortingVariable}
                        filters={this.state.filters}
                        showDifference={this.state.showDifference}
                        nonEmptyFiltersCount={this.getNonEmptyFiltersCount()}
                        updateFilters={this.updateFilters.bind(this)}
                        updateTableVariable={this.updateTableVariable.bind(this)}
                        updateShowDifference={this.updateShowDifference.bind(this)}
                        reset={this.reset.bind(this)}
                        addButton={this.addLeftButton.bind(this)}
                    />
                )}
                {this.state.mode !== null && this.state.mode !== 'advanced' && this.state.mode !== 'no-component' && (
                    <Redirect to={'/' + this.state.mode} />
                )}

                {this.state.groupingVariable && this.state.sortingVariable && this.getNonEmptyFiltersCount() > 1 && (
                    <div>
                        <ResultsDataTable
                            filters={this.state.filters}
                            groupingVariable={this.state.groupingVariable}
                            sortingVariable={this.state.sortingVariable}
                            showDifference={this.state.showDifference}
                            sort={this.state.sort}
                            sortable
                            downloadable
                            addButton={this.addRightButton.bind(this)}
                        />
                    </div>
                )}
            </div>
        );
    }

    home() {
        this.setState({
            sortingVariable: null,
            groupingVariable: null,
            showDifference: false,
            filters: {},
            leftButtons: [],
            rightButtons: [],
            mode: null
        });
    }

    reset() {
        this.setState({
            sortingVariable: null,
            groupingVariable: null,
            showDifference: false,
            filters: {}
        });
    }

    addLeftButton(lb) {
        this.setState(prevState => ({ leftButtons: [...prevState.leftButtons, lb] }));
    }

    addRightButton(rb) {
        this.setState(prevState => ({ rightButtons: [...prevState.rightButtons, rb] }));
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

    async populateExecutionData() {
        // get questions, data groups, executions, possible responses
        // maybe a questions API route for easier processing later
        const response = await Promise.all(
            [
                fetch('api/executions'),
            ]
        );
        const [executions] = await Promise.all(
            response.map(r => r.json())
        );
        const sortedExecutions = executions.sort((a, b) => (a.occurredTime < b.occurredTime) ? 1 : ((a.occurredTime > b.occurredTime ? -1 : 0)));

        this.setState({
            latestExecutionNames: sortedExecutions.map(se => se.key),
            loading: false
        });
    }
}
