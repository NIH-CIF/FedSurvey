import React, { Component } from 'react';
import { Button, ButtonGroup, Row, Col } from 'reactstrap';
import { Link } from 'react-router-dom';
import { Advanced } from './Advanced';
import { ResultsDataTable } from './ResultsDataTable';
import { Redirect } from 'react-router-dom';
import api from '../api';
import { hasToken, LOGIN_ENABLED } from '../token';

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
            views: [],
            leftButtons: [],
            rightButtons: []
        };
    }

    componentDidMount() {
        this.populateHomeData();
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
                            disabled={this.state.mode === null}
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
                        {(hasToken() || !LOGIN_ENABLED) ? (
                            <Button
                                outline
                                color="secondary"
                                tag={Link}
                                to="/admin"
                            >
                                Admin
                            </Button>
                        ) : (
                            <Button
                                outline
                                color="primary"
                                tag={Link}
                                to="/login"
                            >
                                Log In
                            </Button>
                        )}

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
                        {this.state.views.map(v => (
                            this.linkCol(v.name, {mode: 'no-component', ...this.prepareConfig(v.config)})
                        ))}
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
            sort: {},
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
            filters: {},
            sort: {}
        });
    }

    addLeftButton(lb) {
        const existingButtons = this.state.leftButtons;

        if (this.state.leftButtons.map(lb => lb.text).includes(lb.text)) {
            existingButtons.splice(existingButtons.findIndex(l => l.text === lb.text), 1);
        }

        this.setState(prevState => ({ leftButtons: [...existingButtons, lb] }));
    }

    addRightButton(rb) {
        const existingButtons = this.state.rightButtons;

        if (this.state.rightButtons.map(rb => rb.text).includes(rb.text)) {
            existingButtons.splice(existingButtons.findIndex(r => r.text === rb.text), 1);
        }

        this.setState(prevState => ({ rightButtons: [...existingButtons, rb] }));
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

    prepareConfig(json) {
        return JSON.parse(json.replace(/\$\(latestExecutionNames(\d)\)/, (a, b) => {
            return this.state.latestExecutionNames.slice(0, b).join(',');
        }));
    }

    async populateHomeData() {
        // get questions, data groups, executions, possible responses
        // maybe a questions API route for easier processing later
        const response = await Promise.all(
            [
                api.fetch('api/executions'),
                api.fetch('api/views')
            ]
        );
        const [executions, views] = await Promise.all(
            response.map(r => r.json())
        );
        const sortedExecutions = executions.sort((a, b) => (a.occurredTime < b.occurredTime) ? 1 : ((a.occurredTime > b.occurredTime ? -1 : 0)));

        this.setState({
            latestExecutionNames: sortedExecutions.map(se => se.key),
            views,
            loading: false
        });
    }
}
