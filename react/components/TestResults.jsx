import React, { useState, useEffect } from "react";
import {
  Row,
  Col,
  Card,
  Nav,
  Tab,
  Table,
  InputGroup,
  Form,
} from "react-bootstrap";
import TestsTable from "./TestsTable";
import testService from "services/testService";
import Pagination from "rc-pagination";
import "rc-pagination/assets/index.css";
import locale from "rc-pagination/lib/locale/en_US";
import debug from "assignref-debug";
import TitleHeader from "components/general/TitleHeader";

const _logger = debug.extend("Test");

const TestResults = () => {
  //#region State | UseEffect

  const [tableData, setTableData] = useState({
    data: [],
    mappedData: [],
  });

  const [pageData, setPageData] = useState({
    current: 1,
    totalCount: 0,
    pageSize: 10,
    query: "",
    startDate: new Date("January 1 2023").toISOString(),
    endDate: new Date().toISOString(),
  });

  useEffect(() => {
    if (pageData.startDate === "" || pageData.endDate === "") {
      testService
        .searchResults(
          pageData.current - 1,
          pageData.pageSize,
          pageData.query,
          new Date("January 1 2023").toISOString(),
          new Date().toISOString()
        )
        .then(onGetTestSuccess)
        .catch(onGetTestError);
    } else {
      testService
        .searchResults(
          pageData.current - 1,
          pageData.pageSize,
          pageData.query,
          pageData.startDate,
          pageData.endDate
        )
        .then(onGetTestSuccess)
        .catch(onGetTestError);
    }
  }, [pageData.query, pageData.current, pageData.startDate, pageData.endDate]);

  //#endregion

  //#region Functionality

  const rowMapper = (rowData) => {
    return <TestsTable rowData={rowData} key={rowData.id} />;
  };

  const onPageChange = (page) => {
    setPageData((prevState) => {
      const update = { ...prevState };

      update.current = page;

      return update;
    });
  };

  const onChange = (e) => {
    let target = e.target.name;
    let value = e.target.value;

    setPageData((prevState) => {
      const update = { ...prevState };

      update[target] = value;

      return update;
    });
  };

  //#endregion

  //#region Handlers

  const onGetTestSuccess = (response) => {
    setTableData((prevState) => {
      const update = { ...prevState };

      update.data = response.item.pagedItems;

      update.mappedData = response.item.pagedItems.map(rowMapper);

      return update;
    });

    setPageData((prevState) => {
      const update = { ...prevState };

      update.totalCount = response.item.totalCount;

      return update;
    });
  };

  const onGetTestError = (error) => {
    _logger(error);
  };

  //#endregion

  return (
    <>
      <TitleHeader title="Test Results" />
      <Row>
        <Col>
          <Tab.Container defaultActiveKey="all">
            <Card>
              <Card.Header className="border-bottom-0 p-0 bg-white">
                <Nav className="nav-lb-tab">
                  <Nav.Item>
                    <Nav.Link eventKey="all" className="mb-sm-3 mb-md-0">
                      All
                    </Nav.Link>
                  </Nav.Item>
                  <Nav.Item>
                    <Nav.Link eventKey="completed" className="mb-sm-3 mb-md-0">
                      Completed
                    </Nav.Link>
                  </Nav.Item>
                  <Nav.Item>
                    <Nav.Link eventKey="pending" className="mb-sm-3 mb-md-0">
                      Pending
                    </Nav.Link>
                  </Nav.Item>
                </Nav>
              </Card.Header>
              <Card.Body className="p-0">
                <Row>
                  <Col className="mb-lg-0 py-4 px-5">
                    <InputGroup>
                      <Form.Control
                        type="search"
                        className="form-control"
                        name="query"
                        placeholder="Search Tests"
                        onChange={onChange}
                        value={pageData.query}
                      />
                      <Form.Control
                        type="date"
                        name="startDate"
                        value={pageData.startDate}
                        onChange={onChange}
                      />
                      <Form.Control
                        type="date"
                        name="endDate"
                        value={pageData.endDate}
                        onChange={onChange}
                      />
                    </InputGroup>
                  </Col>
                  <Row>
                    <Col className="d-flex justify-content-center mb-3">
                      <Pagination
                        current={pageData.current}
                        total={pageData.totalCount}
                        pageSize={pageData.pageSize}
                        onChange={onPageChange}
                        locale={locale}
                      />
                    </Col>
                  </Row>
                </Row>
                <Tab.Content>
                  <Tab.Pane eventKey="all" className="pb-4">
                    <Table className="text-nowrap" size="sm">
                      <thead className="table-light">
                        <tr role="row" className="text-center">
                          <th colSpan={1} role="columnheader">
                            User Id
                          </th>
                          <th colSpan={1} role="columnheader">
                            Test Taker
                          </th>
                          <th colSpan={1} role="columnheader">
                            Date Started
                          </th>
                          <th colSpan={1} role="columnheader">
                            Test
                          </th>
                          <th colSpan={1} role="columnheader">
                            Test Category
                          </th>
                          <th colSpan={1} role="columnheader">
                            Status
                          </th>
                          <th colSpan={1} role="columnheader">
                            Score
                          </th>
                          <th colSpan={1} role="columnheader" />
                        </tr>
                      </thead>
                      {tableData.mappedData}
                    </Table>
                  </Tab.Pane>
                  <Tab.Pane eventKey="completed" className="pb-4"></Tab.Pane>
                  <Tab.Pane eventKey="pending" className="pb-4"></Tab.Pane>
                </Tab.Content>
              </Card.Body>
            </Card>
          </Tab.Container>
        </Col>
      </Row>
    </>
  );
};

export default TestResults;
