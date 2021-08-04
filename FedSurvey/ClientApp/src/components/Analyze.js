import React, { Component } from 'react';
import { Button, Row, Col } from 'reactstrap';
import { Link } from 'react-router-dom';
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
            sort: {},
            mode: null,
            latestExecutionNames: null
        };
    }

    componentDidMount() {
        this.populateExecutionData();
    }

    // Maybe the hardcoded strings should just be in a file for changes to be made.
    render() {
        return (
            <div style={{ height: '100%' }}>
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

                {this.state.mode === null && (
                    <Row xs="3" style={{ height: '100%' }}>
                        <Col onClick={e => this.setState(
                            {
                                mode: 'no-component',
                                sortingVariable: 'dataGroupName',
                                groupingVariable: 'questionId',
                                filters: {
                                    'possible-response-names': ['Positive'],
                                    'execution-keys': [this.state.latestExecutionNames[0]],
                                    'data-group-names': ['OSC TOTAL', 'DIV PRGM COORD, PLNG & STRATEGIC INITIATIVES', 'OFFICE OF THE DIRECTOR (OD)']
                                },
                                sort: {
                                    header: 'OSC TOTAL',
                                    direction: 'desc'
                                }
                            }
                        )}>
                            Top Positive Responses
                        </Col>
                        <Col onClick={e => this.setState(
                            {
                                mode: 'no-component',
                                sortingVariable: 'dataGroupName',
                                groupingVariable: 'questionId',
                                filters: {
                                    'possible-response-names': ['Neutral'],
                                    'execution-keys': [this.state.latestExecutionNames[0]],
                                    'data-group-names': ['OSC TOTAL', 'DIV PRGM COORD, PLNG & STRATEGIC INITIATIVES', 'OFFICE OF THE DIRECTOR (OD)']
                                },
                                sort: {
                                    header: 'OSC TOTAL',
                                    direction: 'desc'
                                }
                            }
                        )}>
                            Top Neutral Responses
                        </Col>
                        <Col onClick={e => this.setState(
                            {
                                mode: 'no-component',
                                sortingVariable: 'dataGroupName',
                                groupingVariable: 'questionId',
                                filters: {
                                    'possible-response-names': ['Negative'],
                                    'execution-keys': [this.state.latestExecutionNames[0]],
                                    'data-group-names': ['OSC TOTAL', 'DIV PRGM COORD, PLNG & STRATEGIC INITIATIVES', 'OFFICE OF THE DIRECTOR (OD)']
                                },
                                sort: {
                                    header: 'OSC TOTAL',
                                    direction: 'desc'
                                }
                            }
                        )}>
                            Top Negative Responses
                        </Col>
                        <Col onClick={e => this.setState(
                            {
                                mode: 'no-component',
                                sortingVariable: 'executionName',
                                groupingVariable: 'questionId',
                                filters: {
                                    'possible-response-names': ['Positive'],
                                    'data-group-names': ['OSC TOTAL']
                                },
                                showDifference: true,
                                sort: {
                                    header: this.state.latestExecutionNames[0],
                                    direction: 'desc'
                                }
                            }
                        )}>
                            Top Positive Responses Compared to 2016 on
                        </Col>
                        <Col onClick={e => this.setState(
                            {
                                mode: 'no-component',
                                sortingVariable: 'executionName',
                                groupingVariable: 'questionId',
                                filters: {
                                    'possible-response-names': ['Negative'],
                                    'data-group-names': ['OSC TOTAL']
                                },
                                showDifference: true,
                                sort: {
                                    header: this.state.latestExecutionNames[0],
                                    direction: 'desc'
                                }
                            }
                        )}>
                            Top Negative Responses Compared to 2016 on
                        </Col>
                        <Col onClick={e => this.setState(
                            {
                                mode: 'no-component',
                                sortingVariable: 'executionName',
                                groupingVariable: 'questionId',
                                filters: {
                                    'possible-response-names': ['Positive'],
                                    'data-group-names': ['OSC TOTAL'],
                                    'execution-keys': this.state.latestExecutionNames.slice(0, 2)
                                },
                                showDifference: true,
                                sort: {
                                    // show two orgs + delta = last index
                                    index: 2,
                                    direction: 'desc'
                                }
                            }
                        )}>
                            Top Positive Response Increases
                        </Col>
                        <Col onClick={e => this.setState(
                            {
                                mode: 'no-component',
                                sortingVariable: 'executionName',
                                groupingVariable: 'questionId',
                                filters: {
                                    'possible-response-names': ['Positive'],
                                    'data-group-names': ['OSC TOTAL'],
                                    'execution-keys': this.state.latestExecutionNames.slice(0, 2)
                                },
                                showDifference: true,
                                sort: {
                                    // show two orgs + delta = last index
                                    index: 2,
                                    direction: 'asc'
                                }
                            }
                        )}>
                            Top Positive Response Decreases
                        </Col>
                        <Col onClick={e => this.setState(
                            {
                                mode: 'no-component',
                                sortingVariable: 'dataGroupName',
                                groupingVariable: 'questionId',
                                filters: {
                                    'possible-response-names': ['Positive'],
                                    'data-group-names': ['OSC TOTAL', 'DIV PRGM COORD, PLNG & STRATEGIC INITIATIVES'],
                                    'execution-keys': [this.state.latestExecutionNames[0]]
                                },
                                showDifference: true,
                                sort: {
                                    // show two orgs + delta = last index
                                    index: 2,
                                    direction: 'desc'
                                }
                            }
                        )}>
                            Top Positive Response Strengths Relative to DPCPSI
                        </Col>
                        <Col onClick={e => this.setState(
                            {
                                mode: 'no-component',
                                sortingVariable: 'dataGroupName',
                                groupingVariable: 'questionId',
                                filters: {
                                    'possible-response-names': ['Positive'],
                                    'data-group-names': ['OSC TOTAL', 'DIV PRGM COORD, PLNG & STRATEGIC INITIATIVES'],
                                    'execution-keys': [this.state.latestExecutionNames[0]]
                                },
                                showDifference: true,
                                sort: {
                                    // show two orgs + delta = last index
                                    index: 2,
                                    direction: 'asc'
                                }
                            }
                        )}>
                            Top Positive Response Weaknesses Relative to DPCPSI
                        </Col>
                        <Col onClick={e => this.setState({ mode: 'advanced' })}>
                            Advanced
                        </Col>
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
                    />
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
            latestExecutionNames: sortedExecutions.map(se => se.key)
        });
    }
}
