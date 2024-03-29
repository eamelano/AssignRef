import React, { useState, useEffect } from "react";
import PreviewQuestion from "./PreviewQuestion";
import { useParams } from "react-router-dom";
import testService from "services/testService";
import { Button, Row } from "react-bootstrap";
import { Formik, Form, FieldArray } from "formik";
import TitleHeader from "components/general/TitleHeader";

const TestPreview = () => {
  //#region Hooks | Variables

  const { id } = useParams();

  const [test, setTest] = useState({
    id: id,
    testQuestionList: [],
  });

  useEffect(() => {
    testService.getById(id).then(onGetSuccess).catch(onError);
  }, []);

  const initialValues = {
    picked: "",
  };

  //#endregion

  //#region Functionality

  const renderQuestionOptions = ({ form }) => {
    const values = form.values;
    const mapArr = (question, index) => {
      return (
        <PreviewQuestion
          question={question}
          index={index}
          values={values}
          key={question.id}
        />
      );
    };

    return test.testQuestionList.map(mapArr);
  };

  //#endregion

  //#region Handlers

  const handleSubmit = (values) => {
    const mappedAnswers = test.testQuestionList
      .flatMap((arr) => arr.answerOption)
      .filter((arrObj) => Object.values(values)[0].includes(arrObj.text))
      .map((obj) => {
        return {
          answerOptionId: obj.id,
          questionId: obj.questionId,
          answer: obj.text,
        };
      });
    const payload = {
      testId: test.id,
      testAnswers: mappedAnswers,
    };
    testService
      .CreateInstance(payload)
      .then(onCreateSuccess)
      .catch(onCreateError);
  };

  const onCreateSuccess = (response) => {
    console.log(response);
  };

  const onCreateError = (error) => {
    console.log(error);
  };

  const onGetSuccess = (response) => {
    setTest((prevState) => {
      const tst = { ...prevState };
      tst.testQuestionList = response.item.testQuestions;
      tst.testName = response.item.name;
      return tst;
    });
  };

  const onError = (e) => {
    console.log(e);
  };

  //#endregion

  return (
    <div className="col-xl-8 col-md-12 col-12">
      <TitleHeader title={test.testName} />
      <Row>
        <div className="mb-4 col-12">
          <div className="card">
            <div className="card-header card-header bg-secondary">
              <div className="d-flex justify-content-between align-items-center">
                <div>
                  <h4 className="mb-1 text-white"> {test.name}</h4>
                </div>
              </div>
            </div>
            <div className="card-body">
              <Formik initialValues={initialValues} onSubmit={handleSubmit}>
                <Form>
                  <FieldArray>
                    {test.testQuestionList.length > 0 && renderQuestionOptions}
                  </FieldArray>
                  <Button type="submit">Submit</Button>
                </Form>
              </Formik>
            </div>
          </div>
        </div>
      </Row>
    </div>
  );
};

export default TestPreview;
